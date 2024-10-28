cmake_minimum_required(VERSION 3.27)
project(StackOnPointers LANGUAGES C)
enable_testing()

set(CMAKE_LIBRARY_OUTPUT_DIRECTORY ${CMAKE_CURRENT_BINARY_DIR}/bin)
set(CMAKE_RUNTIME_OUTPUT_DIRECTORY ${CMAKE_CURRENT_BINARY_DIR}/bin)


add_executable(testStack tests/tests.c)

add_library(stackOnPointers STATIC stack.c functions.c)
target_link_libraries(testStack stackOnPointers)

add_test(NAME "StackOnPointers Test" COMMAND testStack)

add_custom_command(
    TARGET testStack
    POST_BUILD
    COMMAND ${CMAKE_CTEST_COMMAND}
)
