//
// Created by fabian on 28.03.22.
//

#include "IPCTester.h"
#include <base64.hpp>
#include <ImGuiFileDialog.h>
#include <process.hpp>
#include <nlohmann/json.hpp>
#include <hello_imgui.h>
#include "ProcessControlWindow.h"

void ProcessControlWindow() {
    ImGui::Begin("Process Control");
    static std::string output;
    static char command[100];
    ImGui::InputText("Command", command, 100);
    output.clear();
    if (ImGui::Button("Launch")) {
        output = std::string();
        TinyProcessLib::Process(command, "", [](const char *input, size_t input_size) {
            output.append(input, input_size);
        });
    }
    ImGui::Text("%s", output.c_str());
    ImGui::End();
}