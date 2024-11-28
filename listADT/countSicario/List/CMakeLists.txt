cmake_minimum_required(VERSION 3.27)
project("List" LANGUAGES C)
enable_testing()
include_directories("${PROJECT_SOURCE_DIR}/include")
set(PROJECT_SOURCES "${PROJECT_SOURCE_DIR}/src")
set(TESTS_PATH "${PROJECT_SOURCE_DIR}/tests")
set(CMAKE_C_FLAGS "${CMAKE_C_FLAGS} -Wall -O2")

set(CMAKE_LIBRARY_OUTPUT_DIRECTORY ${CMAKE_CURRENT_BINARY_DIR}/bin)
set(CMAKE_RUNTIME_OUTPUT_DIRECTORY ${CMAKE_CURRENT_BINARY_DIR}/bin)

# project library
add_library(list STATIC ${PROJECT_SOURCES}/list.c ${PROJECT_SOURCES}/circularList.c)

# tests
add_executable(testList ${TESTS_PATH}/tests.c )
add_test(NAME "List test" COMMAND testList)
target_link_libraries(testList PUBLIC list)
target_compile_definitions(testList PRIVATE RANDOM_DATA_PATH="${TESTS_PATH}/randomData")

# coverage test
option(CODE_COVERAGE "Generate code coverage" OFF)
if(CMAKE_COMPILER_IS_GNUCC AND CODE_COVERAGE)
list(APPEND CMAKE_MODULE_PATH "${PROJECT_SOURCE_DIR}/cmake_modules")
include(CodeCoverage)
APPEND_COVERAGE_COMPILER_FLAGS()
SETUP_TARGET_FOR_COVERAGE_LCOV(NAME ListCoverage EXECUTABLE testList)
endif()
