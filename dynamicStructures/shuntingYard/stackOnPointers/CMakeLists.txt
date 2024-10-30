cmake_minimum_required(VERSION 3.27)
project(StackOnPointers LANGUAGES C)
enable_testing()

set(CMAKE_LIBRARY_OUTPUT_DIRECTORY ${CMAKE_CURRENT_BINARY_DIR}/bin)
set(CMAKE_RUNTIME_OUTPUT_DIRECTORY ${CMAKE_CURRENT_BINARY_DIR}/bin)


add_executable(testStack tests/tests.c ../tokens.c)

add_library(stackOnPointers STATIC stack.c ../memoryAuxiliaries.c)
target_link_libraries(testStack stackOnPointers)

add_test(NAME "StackOnPointers Test" COMMAND testStack)
