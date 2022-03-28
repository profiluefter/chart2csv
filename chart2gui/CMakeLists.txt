cmake_minimum_required(VERSION 2.8.12)
project(chart2gui)
set(CMAKE_CXX_STANDARD 17)

set(HELLOIMGUI_USE_SDL_OPENGL3 ON)

add_subdirectory(lib/hello_imgui)
set(CMAKE_MODULE_PATH ${CMAKE_MODULE_PATH} ${HELLOIMGUI_BASEPATH}/hello_imgui_cmake)
include(hello_imgui_add_app)

hello_imgui_add_app(chart2gui src/main.cpp lib/imgui-file-dialog/ImGuiFileDialog.cpp)

add_library(json INTERFACE)
target_include_directories(json INTERFACE lib/json)
target_link_libraries(chart2gui PRIVATE json)

add_library(imgui-file-dialog INTERFACE)
target_include_directories(imgui-file-dialog INTERFACE lib/imgui-file-dialog)
target_link_libraries(chart2gui PRIVATE imgui-file-dialog)

add_library(imgui-headers INTERFACE)
target_include_directories(imgui-headers INTERFACE lib/hello_imgui/external/imgui)

add_library(imgui-node-editor OBJECT lib/imgui-node-editor/crude_json.cpp lib/imgui-node-editor/imgui_canvas.cpp lib/imgui-node-editor/imgui_node_editor.cpp lib/imgui-node-editor/imgui_node_editor_api.cpp)
target_include_directories(imgui-node-editor PUBLIC lib/imgui-node-editor/)
target_link_libraries(imgui-node-editor PRIVATE imgui-headers)
target_link_libraries(chart2gui PRIVATE imgui-node-editor)

add_library(libbase64 OBJECT lib/libbase64/src/base64_decode.cpp lib/libbase64/src/base64_encode.cpp)
target_include_directories(libbase64 PUBLIC lib/libbase64/include)
target_link_libraries(chart2gui PRIVATE libbase64)

add_subdirectory(lib/tiny-process-library)
target_link_libraries(chart2gui PRIVATE tiny-process-library)

add_custom_target(chart2csv
        COMMAND dotnet publish -c Release -r linux-x64 --self-contained
        WORKING_DIRECTORY ${PROJECT_SOURCE_DIR}/../chart2csv.IPC
        VERBATIM
)

add_custom_command(
        TARGET chart2gui POST_BUILD
        COMMAND cp -f ${PROJECT_SOURCE_DIR}/../chart2csv.IPC/bin/Release/net6.0/linux-x64/publish/chart2csv.IPC ${CMAKE_BINARY_DIR}
        BYPRODUCTS ${CMAKE_BINARY_DIR}/chart2csv.IPC
        VERBATIM
)

add_dependencies(chart2gui chart2csv)