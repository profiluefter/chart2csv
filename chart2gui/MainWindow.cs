using ImGuiNET;

namespace chart2gui;

public class MainWindow
{
    public void DrawMenuBar()
    {
        if (!ImGui.BeginMenuBar()) return;
        if (ImGui.BeginMenu("File"))
        {
            ImGui.MenuItem("Open...");
            ImGui.Separator();
            ImGui.MenuItem("Close");
            ImGui.EndMenu();
        }
        ImGui.EndMenuBar();
    }
    
    public void Draw()
    {
        ImGui.Begin("Image");
        ImGui.Text("Hi");
        ImGui.End();
    }
}
