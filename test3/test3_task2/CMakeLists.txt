cmake_minimum_required(VERSION 3.27)
project("Hashtable" LANGUAGES C)
enable_testing()
include_directories("${PROJECT_SOURCE_DIR}/include")
set(SOURCES_PATH "${PROJECT_SOURCE_DIR}/src")
set(TESTS_PATH "${PROJECT_SOURCE_DIR}/tests")
set(CMAKE_C_FLAGS "${CMAKE_C_FLAGS} -Wall -O2")

set(CMAKE_LIBRARY_OUTPUT_DIRECTORY ${CMAKE_CURRENT_BINARY_DIR}/bin)
set(CMAKE_RUNTIME_OUTPUT_DIRECTORY ${CMAKE_CURRENT_BINARY_DIR}/bin)

set(PROJECT_SOURCES ${SOURCES_PATH}/tree.c)

# project
add_executable(main  ${SOURCES_PATH}/main.c ${PROJECT_SOURCES})

# tests
add_executable(treeSortTest ${TESTS_PATH}/tests.c ${PROJECT_SOURCES})
add_test(NAME "Tree Sort test" COMMAND treeSortTest)
