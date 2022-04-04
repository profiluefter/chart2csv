#include <hello_imgui.h>
#include <nlohmann/json.hpp>
#include <ImGuiFileDialog.h>
#include <base64.hpp>

#include "IPCTester.h"
#include "ProcessControlWindow.h"

using json = nlohmann::json;

int main() {
    auto params = HelloImGui::RunnerParams();

    params.callbacks.SetupImGuiConfig = [] {
        ImGui::GetIO().ConfigFlags |= ImGuiConfigFlags_DockingEnable;
        ImGui::GetIO().ConfigFlags |= ImGuiConfigFlags_ViewportsEnable;
    };

    params.callbacks.ShowGui = [] {
        ImGui::DockSpace(ImGui::GetID("dockspace"));

        IPCTester();
        ProcessControlWindow();

        if (ImGuiFileDialog::Instance()->Display("open-file")) {
            if (ImGuiFileDialog::Instance()->IsOk()) {
                auto filePathName = ImGuiFileDialog::Instance()->GetFilePathName();
                std::cout << filePathName << std::endl;
            }
            ImGuiFileDialog::Instance()->Close();
        }
    };

    params.callbacks.ShowMenus = [] {
        if (ImGui::BeginMenu("File")) {
            if (ImGui::MenuItem("Open...")) {
                ImGuiFileDialog::Instance()->OpenDialog("open-file", "Open an input file", ".png", ".");
            }
            ImGui::EndMenu();
        }
    };

    params.appWindowParams.windowSize = {1000.f, 500.f};
    params.appWindowParams.windowTitle = "chart2csv Studio";
    params.imGuiWindowParams.showMenuBar = true;

    HelloImGui::Run(params);
    return 0;
}
