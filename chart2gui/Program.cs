using System.Numerics;
using chart2gui;
using ImGuiNET;
using Veldrid;
using Veldrid.StartupUtilities;

void DrawDockSpace(Action drawMenuBar)
{
    var viewport = ImGui.GetMainViewport();
    ImGui.SetNextWindowPos(viewport.WorkPos);
    ImGui.SetNextWindowSize(viewport.WorkSize);
    ImGui.SetNextWindowViewport(viewport.ID);
    ImGui.PushStyleVar(ImGuiStyleVar.WindowRounding, 0.0f);
    ImGui.PushStyleVar(ImGuiStyleVar.WindowBorderSize, 0.0f);
    ImGui.PushStyleVar(ImGuiStyleVar.WindowPadding, Vector2.Zero);

    ImGui.Begin("DockSpace", ImGuiWindowFlags.NoTitleBar | ImGuiWindowFlags.NoCollapse |
                             ImGuiWindowFlags.NoResize | ImGuiWindowFlags.NoMove |
                             ImGuiWindowFlags.NoBringToFrontOnFocus | ImGuiWindowFlags.NoNavFocus |
                             ImGuiWindowFlags.MenuBar);

    ImGui.PopStyleVar(3);

    drawMenuBar();

    ImGui.DockSpace(ImGui.GetID("DockSpace"));
    ImGui.End();
}

VeldridStartup.CreateWindowAndGraphicsDevice(
    new WindowCreateInfo(100, 100, 1280, 720, WindowState.Normal, "Chart2GUI"),
    out var window,
    out var gd);

ImGuiRenderer imguiRenderer = new ImGuiRenderer(
    gd, gd.MainSwapchain.Framebuffer.OutputDescription,
    (int)gd.MainSwapchain.Framebuffer.Width, (int)gd.MainSwapchain.Framebuffer.Height);
var cl = gd.ResourceFactory.CreateCommandList();

ImGui.GetIO().ConfigFlags |= ImGuiConfigFlags.DockingEnable;
ImGui.GetIO().ConfigFlags |= ImGuiConfigFlags.ViewportsEnable;

var mainWindow = new MainWindow();

while (window.Exists)
{
    var input = window.PumpEvents();
    if (!window.Exists)
    {
        break;
    }

    imguiRenderer.Update(1f / 60f, input); // Compute actual value for deltaSeconds.

    // Draw dockspace
    DrawDockSpace(mainWindow.DrawMenuBar);

    // Draw UI
    mainWindow.Draw();

    cl.Begin();
    cl.SetFramebuffer(gd.MainSwapchain.Framebuffer);
    cl.ClearColorTarget(0, RgbaFloat.Black);
    imguiRenderer.Render(gd, cl);
    cl.End();
    gd.SubmitCommands(cl);
    gd.SwapBuffers(gd.MainSwapchain);
}
