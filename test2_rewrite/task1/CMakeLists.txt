cmake_minimum_required(VERSION 3.27)
project("Test 2 Task 1 rewrite" LANGUAGES C)
enable_testing()
include_directories("${PROJECT_SOURCE_DIR}/include")
set(SOURCES_PATH "${PROJECT_SOURCE_DIR}/src")
set(TESTS_PATH "${PROJECT_SOURCE_DIR}/tests")
set(CMAKE_C_FLAGS "${CMAKE_C_FLAGS} -Wall -O2")

set(CMAKE_LIBRARY_OUTPUT_DIRECTORY ${CMAKE_CURRENT_BINARY_DIR}/bin)
set(CMAKE_RUNTIME_OUTPUT_DIRECTORY ${CMAKE_CURRENT_BINARY_DIR}/bin)

set(PROJECT_SOURCES ${SOURCES_PATH}/processFile.c)

# project
add_subdirectory(List)
add_executable(main ${SOURCES_PATH}/main.c ${PROJECT_SOURCES})
target_link_libraries(main PRIVATE list)

# tests
add_executable(processFileTest ${TESTS_PATH}/tests.c ${PROJECT_SOURCES})
target_link_libraries(processFileTest PRIVATE list)
add_test(NAME "Test 2 Task 1 rewrite Test" COMMAND processFileTest)
target_compile_definitions(processFileTest PRIVATE TEST_IN="${TESTS_PATH}/testIn" TEST_OUT="${TESTS_PATH}/testOut")