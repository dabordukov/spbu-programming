cmake_minimum_required(VERSION 3.27)
project("Count Sicario" LANGUAGES C)
enable_testing()
include_directories("${PROJECT_SOURCE_DIR}/include")
set(SOURCES_PATH "${PROJECT_SOURCE_DIR}/src")
set(TESTS_PATH "${PROJECT_SOURCE_DIR}/tests")
set(CMAKE_C_FLAGS "${CMAKE_C_FLAGS} -Wall -g")

set(CMAKE_LIBRARY_OUTPUT_DIRECTORY ${CMAKE_CURRENT_BINARY_DIR}/bin)
set(CMAKE_RUNTIME_OUTPUT_DIRECTORY ${CMAKE_CURRENT_BINARY_DIR}/bin)

set(PROJECT_SOURCES ${SOURCES_PATH}/ioAuxiliaries.c ${SOURCES_PATH}/sortedList.c)

# project
add_executable(main ${SOURCES_PATH}/main.c ${PROJECT_SOURCES})
target_link_libraries(main PUBLIC list)

add_subdirectory(List)

# tests
add_executable(mainTest ${TESTS_PATH}/tests.c ${SOURCES_PATH}/sortedList.c)
add_test(NAME "Sorted List test" COMMAND mainTest)
target_link_libraries(mainTest PUBLIC list)
