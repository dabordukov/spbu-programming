cmake_minimum_required(VERSION 3.27)
project(ShuntingYard LANGUAGES C)

include_directories("${PROJECT_SOURCE_DIR}/include")
set(PROJECT_SOURCES "${PROJECT_SOURCE_DIR}/src")
set(TESTS_PATH "${PROJECT_SOURCE_DIR}/tests")
set(CMAKE_C_FLAGS "${CMAKE_C_FLAGS} -Wall -O2")

enable_testing()

set(CMAKE_LIBRARY_OUTPUT_DIRECTORY ${CMAKE_CURRENT_BINARY_DIR}/bin)
set(CMAKE_RUNTIME_OUTPUT_DIRECTORY ${CMAKE_CURRENT_BINARY_DIR}/bin)

set(MAIN_SOURCES "${PROJECT_SOURCES}/shuntingYard.c" "${PROJECT_SOURCES}/tokens.c")
set(TEST_SOURCES "${PROJECT_SOURCES}/tests.c" "${PROJECT_SOURCES}/tokens.c")

add_library(shuntingYard STATIC ${MAIN_SOURCES})
add_subdirectory(stackOnPointers)
add_subdirectory(queue)
target_link_libraries(shuntingYard PRIVATE queue PRIVATE stackOnPointers)

add_executable(testShuntingYard ${TESTS_PATH}/tests.c)
target_link_libraries(testShuntingYard PRIVATE shuntingYard)

add_executable(main ${PROJECT_SOURCES}/main.c ${PROJECT_SOURCES}/ioAuxiliaries.c)
target_link_libraries(main PRIVATE shuntingYard)

add_test(NAME "Test Shunting Yard" COMMAND testShuntingYard)
