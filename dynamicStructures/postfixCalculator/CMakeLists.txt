cmake_minimum_required(VERSION 3.27)
project(PostfixCalculator LANGUAGES C)
enable_testing()

set(CMAKE_LIBRARY_OUTPUT_DIRECTORY ${CMAKE_CURRENT_BINARY_DIR}/bin)
set(CMAKE_RUNTIME_OUTPUT_DIRECTORY ${CMAKE_CURRENT_BINARY_DIR}/bin)

set(PROJECT_SOURCES "main.c" "postfixCalculator.c" )

add_executable(main ${PROJECT_SOURCES})

add_subdirectory(stackOnPointers)
