cmake_minimum_required(VERSION 3.27)
project("List" LANGUAGES C)
enable_testing()

include_directories("${PROJECT_SOURCE_DIR}/include")
set(PROJECT_SOURCES "${PROJECT_SOURCE_DIR}/src")
set(TESTS_PATH "${PROJECT_SOURCE_DIR}/tests")
set(CMAKE_C_FLAGS "${CMAKE_C_FLAGS} -Wall -g")

set(CMAKE_LIBRARY_OUTPUT_DIRECTORY ${CMAKE_CURRENT_BINARY_DIR}/bin)
set(CMAKE_RUNTIME_OUTPUT_DIRECTORY ${CMAKE_CURRENT_BINARY_DIR}/bin)

add_executable(testList ${TESTS_PATH}/tests.c)
add_test(NAME "List test" COMMAND testList)

add_library(list STATIC ${PROJECT_SOURCES}/list.c)
target_link_libraries(testList list)
