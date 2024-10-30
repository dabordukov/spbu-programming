cmake_minimum_required(VERSION 3.27)
project(ShuntingYard LANGUAGES C)
set(CMAKE_C_FLAGS "${CMAKE_C_FLAGS} -Wall -O2")

enable_testing()

set(CMAKE_LIBRARY_OUTPUT_DIRECTORY ${CMAKE_CURRENT_BINARY_DIR}/bin)
set(CMAKE_RUNTIME_OUTPUT_DIRECTORY ${CMAKE_CURRENT_BINARY_DIR}/bin)

set(PROJECT_SOURCES "shuntingYard.c" "tokens.c")
set(TEST_SOURCES "tests.c" "tokens.c")

add_library(shuntingYard STATIC ${PROJECT_SOURCES})
add_subdirectory(stackOnPointers)
add_subdirectory(queue)
target_link_libraries(shuntingYard PRIVATE queue PRIVATE stackOnPointers)

add_executable(testShuntingYard tests.c)
target_link_libraries(testShuntingYard PRIVATE shuntingYard)

add_executable(main main.c ioAuxiliaries.c)
target_link_libraries(main PRIVATE shuntingYard)

add_test(NAME "Test Shunting Yard" COMMAND testShuntingYard)
