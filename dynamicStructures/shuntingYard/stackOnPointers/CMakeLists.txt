cmake_minimum_required(VERSION 3.27)
project(StackOnPointers LANGUAGES C)
enable_testing()

include_directories(${PROJECT_SOURCE_DIR}/include)
message("${PROJECT_SOURCE_DIR}/include")
set(PROJECT_SOURCES "${PROJECT_SOURCE_DIR}/src")
set(TESTS_PATH "${PROJECT_SOURCE_DIR}/tests")

set(CMAKE_LIBRARY_OUTPUT_DIRECTORY ${CMAKE_CURRENT_BINARY_DIR}/bin)
set(CMAKE_RUNTIME_OUTPUT_DIRECTORY ${CMAKE_CURRENT_BINARY_DIR}/bin)


add_executable(testStack ${TESTS_PATH}/tests.c ${PROJECT_SOURCES}/tokens.c)

add_library(stackOnPointers STATIC ${PROJECT_SOURCES}/stack.c ${PROJECT_SOURCES}/memoryAuxiliaries.c)
target_link_libraries(testStack stackOnPointers)

add_test(NAME "StackOnPointers Test" COMMAND testStack)
