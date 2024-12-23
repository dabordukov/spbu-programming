cmake_minimum_required(VERSION 3.27)
project(OrientedGraph LANGUAGES C)
enable_testing()
include_directories(${PROJECT_SOURCE_DIR}/include)
set(SOURCES_PATH "${PROJECT_SOURCE_DIR}/src")
set(TESTS_PATH "${PROJECT_SOURCE_DIR}/tests")
set(CMAKE_C_FLAGS "${CMAKE_C_FLAGS} -Wall -g")

set(CMAKE_LIBRARY_OUTPUT_DIRECTORY ${CMAKE_CURRENT_BINARY_DIR}/bin)
set(CMAKE_RUNTIME_OUTPUT_DIRECTORY ${CMAKE_CURRENT_BINARY_DIR}/bin)

set(PROJECT_SOURCES ${SOURCES_PATH}/graph.c)

# project
add_executable(main ${SOURCES_PATH}/main.c ${SOURCES_PATH}/ioAuxiliaries.c ${PROJECT_SOURCES} )

# tests
add_executable(testGraph ${TESTS_PATH}/testGraph.c ${PROJECT_SOURCES})
add_compile_definitions(testGraph PRIVATE TEST_FILENAME="${TESTS_PATH}/graph1" )
add_test(NAME "test_OrientedGraph" COMMAND testGraph)
