#include <hello_imgui.h>
#include <nlohmann/json.hpp>
#include <process.hpp>
#include <ImGuiFileDialog.h>
#include <base64.hpp>

using json = nlohmann::json;

void ProcessControlWindow();
void TestWindow();

void display_json_output(const std::vector<json> &jsonOutputs);
void recurse_json_imgui_tree(const json &json, const std::string &name);
int main() {
    auto params = HelloImGui::RunnerParams();

    params.callbacks.SetupImGuiConfig = [] {
        ImGui::GetIO().ConfigFlags |= ImGuiConfigFlags_DockingEnable;
        ImGui::GetIO().ConfigFlags |= ImGuiConfigFlags_ViewportsEnable;
    };

    params.callbacks.ShowGui = [] {
        ImGui::DockSpace(ImGui::GetID("dockspace"));

        TestWindow();
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

void TestWindow() {
    static std::string inputLocation;
    static std::string inputName;
    static char step[100] = "FindOriginStep";
    static std::string input;
//    static std::string inputBuffer;
    static std::string output;
    static std::vector<json> jsonOutputs;

    ImGui::Begin("IPC Tester");
    ImGui::Text("Input File");
    ImGui::SameLine();
    if (ImGui::Button("Pick")) {
        ImGuiFileDialog::Instance()->OpenDialog("ipc-input", "Open an input file", ".png",
                                                "../../chart2csv.Test/charts/");
    }
    ImGui::Text("Selection: %s", inputName.c_str());

    ImGui::InputText("Step", step, 100);

    if (ImGui::Button("Execute")) {
        std::fstream inputImageStream(inputLocation, std::ios_base::in | std::ios_base::binary);

        std::vector<unsigned char> inputImageBuffer(std::istreambuf_iterator<char>(inputImageStream), {});
        std::string inputImageString(reinterpret_cast<const char *>(&inputImageBuffer[0]), inputImageBuffer.size());
        std::string inputImageBase64 = Base64::encode(inputImageString);

        json inputJson;
        inputJson["step"] = std::string(step);
        inputJson["input"]["type"] = "InitialState";
        inputJson["input"]["data"]["InputImage"] = inputImageBase64;
        input = inputJson.dump();

        output.clear();
        auto proc = TinyProcessLib::Process("./chart2csv.IPC", "", [](const char *out, size_t out_size) {
            output.append(out, out_size);
        }, nullptr, true);

        proc.write(input);
        proc.close_stdin();

        if (proc.get_exit_status()) {
            std::cerr << "Invalid exit code " << proc.get_exit_status() << std::endl;
        } else {
            json outputJson = json::parse(output);
            jsonOutputs.push_back(outputJson);
        }
    }

    ImGui::Separator();
    ImGui::Text("Input:\n%s", input.c_str());
    ImGui::Separator();
    ImGui::Text("Output:\n%s", output.c_str());

    ImGui::End();

    display_json_output(jsonOutputs);

    if (ImGuiFileDialog::Instance()->Display("ipc-input")) {
        if (ImGuiFileDialog::Instance()->IsOk()) {
            inputLocation = ImGuiFileDialog::Instance()->GetFilePathName();
            inputName = ImGuiFileDialog::Instance()->GetCurrentFileName();
        }
        ImGuiFileDialog::Instance()->Close();
    }
}
void display_json_output(const std::vector<json> &jsonOutputs) {
    for (int i = 0; i < jsonOutputs.size(); ++i) {
        std::string windowName = "Output ";
        windowName += std::to_string(i);
        ImGui::Begin(windowName.c_str());

        auto jsonOutput = jsonOutputs[i];

        recurse_json_imgui_tree(jsonOutput, "root");

        ImGui::End();
    }
}

void recurse_json_imgui_tree(const json &json, const std::string &name) {
    if (!ImGui::TreeNode(name.c_str()))
        return;

    switch (json.type()) {
        case nlohmann::detail::value_t::null:
            if (ImGui::TreeNode("null"))
                ImGui::TreePop();
            break;
        case nlohmann::detail::value_t::object:
            for (const auto &item: json.items()) {
                recurse_json_imgui_tree(item.value(), item.key());
            }
            break;
        case nlohmann::detail::value_t::array:
            for (int i = 0; i < json.size(); ++i) {
                recurse_json_imgui_tree(json[i], std::to_string(i));
            }
            break;
        case nlohmann::detail::value_t::string:
            ImGui::BulletText("%s", std::string(json).c_str());
            break;
        case nlohmann::detail::value_t::boolean:
            if (ImGui::TreeNode(json ? "true" : "false"))
                ImGui::TreePop();
            break;
        case nlohmann::detail::value_t::number_integer:
            if (ImGui::TreeNode(std::to_string(static_cast<int>(json)).c_str()))
                ImGui::TreePop();
            break;
        case nlohmann::detail::value_t::number_unsigned:
            if (ImGui::TreeNode(std::to_string(static_cast<unsigned int>(json)).c_str()))
                ImGui::TreePop();
            break;
        case nlohmann::detail::value_t::number_float:
            if (ImGui::TreeNode(std::to_string(static_cast<float>(json)).c_str()))
                ImGui::TreePop();
            break;
        case nlohmann::detail::value_t::binary:
            break;
        case nlohmann::detail::value_t::discarded:
            break;
    }

    ImGui::TreePop();
}

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
