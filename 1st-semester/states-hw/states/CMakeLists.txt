cmake_minimum_required(VERSION 3.27)
project("Graph" LANGUAGES C)
enable_testing()
include_directories("${PROJECT_SOURCE_DIR}/include")
set(SOURCES_PATH "${PROJECT_SOURCE_DIR}/src")
set(TESTS_PATH "${PROJECT_SOURCE_DIR}/tests")
set(CMAKE_C_FLAGS "${CMAKE_C_FLAGS} -Wall -O2")

set(CMAKE_LIBRARY_OUTPUT_DIRECTORY ${CMAKE_CURRENT_BINARY_DIR}/bin)
set(CMAKE_RUNTIME_OUTPUT_DIRECTORY ${CMAKE_CURRENT_BINARY_DIR}/bin)

set(PROJECT_SOURCES ${SOURCES_PATH}/graph.c)

# project
add_executable(main ${SOURCES_PATH}/main.c ${SOURCES_PATH}/ioAuxiliaries.c ${PROJECT_SOURCES})

# tests
add_executable(graphTest ${TESTS_PATH}/tests.c ${SOURCES_PATH}/ioAuxiliaries.c ${PROJECT_SOURCES})
target_compile_definitions(graphTest PRIVATE TEST_FILE1="${TESTS_PATH}/graph" TEST_FILE2="${TESTS_PATH}/graph2")
add_test(NAME "Graph test" COMMAND graphTest)
