using Microsoft.Playwright;
using System.Diagnostics;

namespace Delivery.SystemTests;

public class DeliveryTests : IAsyncLifetime
{
    private const string BaseUrl = "https://localhost:7194";

    private IPlaywright _playwright = null!;
    private IBrowser _browser = null!;
    private IBrowserContext _context = null!;
    private IPage _page = null!;
    private Process _webAppProcess = null!;

    public async Task InitializeAsync()
    {
        _webAppProcess = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "dotnet",
                Arguments = "run --no-launch-profile --urls=https://localhost:7194",

                WorkingDirectory =
                    @"C:\Delivery.Web\Delivery.Web",

                RedirectStandardOutput = true,
                RedirectStandardError = true,

                UseShellExecute = false,
                CreateNoWindow = true
            }
        };

        _webAppProcess.Start();

        using var httpClient = new HttpClient(
            new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback =
                    HttpClientHandler
                        .DangerousAcceptAnyServerCertificateValidator
            });

        bool applicationStarted = false;

        for (int i = 0; i < 30; i++)
        {
            if (_webAppProcess.HasExited)
            {
                var erro =
                    await _webAppProcess.StandardError.ReadToEndAsync();

                var saida =
                    await _webAppProcess.StandardOutput.ReadToEndAsync();

                throw new Exception(
                    $"A aplicação não iniciou.\n\n" +
                    $"ERRO:\n{erro}\n\n" +
                    $"SAÍDA:\n{saida}");
            }

            try
            {
                var response =
                    await httpClient.GetAsync(BaseUrl);

                applicationStarted = true;
                break;
            }
            catch
            {
                await Task.Delay(500);
            }
        }

        if (!applicationStarted)
        {
            throw new Exception(
                "A aplicação não respondeu em " +
                $"{BaseUrl} após 15 segundos.");
        }

        _playwright =
            await Playwright.CreateAsync();

        _browser =
            await _playwright.Chromium.LaunchAsync(
                new BrowserTypeLaunchOptions
                {
                    Headless = false
                });

        _context =
            await _browser.NewContextAsync(
                new BrowserNewContextOptions
                {
                    IgnoreHTTPSErrors = true
                });

        _page =
            await _context.NewPageAsync();
    }

    public async Task DisposeAsync()
    {
        if (_page != null)
            await _page.CloseAsync();

        if (_context != null)
            await _context.DisposeAsync();

        if (_browser != null)
            await _browser.DisposeAsync();

        _playwright?.Dispose();

        if (_webAppProcess != null &&
            !_webAppProcess.HasExited)
        {
            try
            {
                _webAppProcess.Kill(true);

                await _webAppProcess.WaitForExitAsync();
            }
            catch
            {
            }
        }

        _webAppProcess?.Dispose();
    }

    //teste de login
    [Fact]
    public async Task Usuario_Deve_Conseguir_Fazer_Login()
    {
        await _page.GotoAsync(
            "https://localhost:7194/Account/Login");

        await _page
            .Locator("#email")
            .FillAsync("joao@email.com");

        await _page
            .Locator("#password")
            .FillAsync("123456");

        await _page
            .Locator("#loginButton")
            .ClickAsync();

        await _page.WaitForLoadStateAsync(
            LoadState.NetworkIdle);

        Assert.Contains(
            "Delivery",
            await _page.TitleAsync());
    }

    // testando o fluxo completo
    [Fact]
    public async Task Usuario_Deve_Conseguir_Realizar_Um_Pedido()
    {
        // 1. Login
        await _page.GotoAsync(
            "https://localhost:7194/Account/Login");

        await _page
            .Locator("#email")
            .FillAsync("joao@email.com");

        await _page
            .Locator("#password")
            .FillAsync("123456");

        await _page
            .Locator("#loginButton")
            .ClickAsync();

        // 2. Selecionar restaurante
        await _page
            .Locator(".menu-link")
            .First
            .ClickAsync();

        // 3. Selecionar pagamento
        await _page
            .Locator(".payment")
            .First
            .SelectOptionAsync("PIX");

        // 4. Confirmar pedido
        await _page
            .Locator(".confirm-order")
            .First
            .ClickAsync();

        // 5. Verificar acompanhamento
        await _page.WaitForLoadStateAsync(
            LoadState.NetworkIdle);

        var status =
            await _page
                .Locator("#orderStatus")
                .InnerTextAsync();

        Assert.Equal(
            "Confirmado",
            status);
    }

    // testando o acompanhamento do pedido
    [Fact]
    public async Task Pedido_Confirmado_Deve_Aparecer_No_Acompanhamento()
    {
        await _page.GotoAsync(
            "https://localhost:7194/Account/Login");

        await _page
            .Locator("#email")
            .FillAsync("joao@email.com");

        await _page
            .Locator("#password")
            .FillAsync("123456");

        await _page
            .Locator("#loginButton")
            .ClickAsync();

        await _page
            .Locator(".menu-link")
            .First
            .ClickAsync();

        await _page
            .Locator(".payment")
            .First
            .SelectOptionAsync("PIX");

        await _page
            .Locator(".confirm-order")
            .First
            .ClickAsync();

        var orderId =
            await _page
                .Locator("#orderId")
                .InnerTextAsync();

        var status =
            await _page
                .Locator("#orderStatus")
                .InnerTextAsync();

        Assert.StartsWith("#", orderId);

        Assert.Equal(
            "Confirmado",
            status);
    }

    //teste de usabilidade
    [Fact]
    public async Task Botao_Confirmar_Pedido_Deve_Estar_Visivel()
    {
        await _page.SetViewportSizeAsync(
            375,
            667);

        await _page.GotoAsync(
            "https://localhost:7194/Account/Login");

        await _page
            .Locator("#email")
            .FillAsync("joao@email.com");

        await _page
            .Locator("#password")
            .FillAsync("123456");

        await _page
            .Locator("#loginButton")
            .ClickAsync();

        await _page
            .Locator(".menu-link")
            .First
            .ClickAsync();

        var button =
            _page
                .Locator(".confirm-order")
                .First;

        Assert.True(
            await button.IsVisibleAsync());
    }

    // teste de performance simples
    [Fact]
    public async Task Cardapio_Deve_Carregar_Em_Tempo_Aceitavel()
    {
        var stopwatch =
            System.Diagnostics.Stopwatch.StartNew();

        await _page.GotoAsync(
            "https://localhost:7194/Home/Menu/1");

        await _page.WaitForLoadStateAsync(
            LoadState.NetworkIdle);

        stopwatch.Stop();

        Assert.True(
            stopwatch.ElapsedMilliseconds < 2000,
            $"Página demorou {stopwatch.ElapsedMilliseconds} ms.");
    }
}
