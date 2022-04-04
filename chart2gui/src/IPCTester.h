//
// Created by fabian on 28.03.22.
//

#ifndef CHART2GUI_IPCTESTER_H
#define CHART2GUI_IPCTESTER_H

#include <nlohmann/json.hpp>

using json = nlohmann::json;

void IPCTester();

void display_json_output(const std::vector<json> &jsonOutputs);

void recurse_json_imgui_tree(const json &json, const std::string &name);

#endif //CHART2GUI_IPCTESTER_H
