"use client"

import { Button } from "@/components/ui/button"
import { Plus, Edit, Trash2 } from "lucide-react"
import { ScheduleLegend } from "./schedule-legend"

export function ScheduleGrid({
                               days,
                               timeSlots,
                               courses,
                               mode,
                               selectedCell,
                               handleCellClick,
                               isTimeSlotOccupied,
                               setSelectedCourse,
                               setIsEditDialogOpen,
                               setIsDeleteDialogOpen,
                             }) {
  const getTypeColor = (type) => {
    switch (type) {
      case "CM": return "bg-purple-200 border-purple-300"
      case "TD": return "bg-yellow-200 border-yellow-300"
      case "TP": return "bg-green-200 border-green-300"
      default: return "bg-gray-200 border-gray-300"
    }
  }

  const getTypeLabelColor = (type) => {
    switch (type) {
      case "CM": return "bg-purple-300"
      case "TD": return "bg-yellow-300"
      case "TP": return "bg-green-300"
      default: return "bg-gray-300"
    }
  }

  const getCourseAtTime = (day, time) => {
    return courses.find((course) => course.day === day && course.startTime === time)
  }

  const getCourseTimeSpan = (startTime, endTime) => {
    const start = timeSlots.indexOf(startTime)
    const end = timeSlots.indexOf(endTime)
    return end - start
  }

  return (
      <div>
        <div className="overflow-auto">
          <table className="m-3">
            <thead>
            <tr>
              <th className="border bg-gray-200 p-2 text-left text-sm font-medium" style={{ minWidth: "40px" }}></th>
              {days.map((day, index) => (
                  <th key={index} className="border bg-gray-200 p-2 text-center text-sm font-medium" style={{ minWidth: "150px" }}>
                    <div>{day.name}</div>
                    <div>{day.date}</div>
                  </th>
              ))}
            </tr>
            </thead>
            <tbody>
            {timeSlots.map((time, timeIndex) => {
              const skipRendering =
                  timeIndex > 0 &&
                  timeIndex < timeSlots.length - 1 &&
                  days.some((_, dayIndex) => {
                    const prevCourse = getCourseAtTime(dayIndex, timeSlots[timeIndex - 1])
                    return prevCourse && prevCourse.endTime !== time
                  })

              if (skipRendering) return null

              return (
                  <tr key={timeIndex} className="h-7">
                    <td className="border bg-gray-100 p-1 text-center text-xs font-medium">{time}</td>
                    {days.map((_, dayIndex) => {
                      const course = getCourseAtTime(dayIndex, time)

                      if (!course) {
                        return (
                            <td
                                key={dayIndex}
                                className={`border p-1 ${mode === "create" ? "cursor-pointer hover:bg-blue-50" : ""}`}
                                onClick={() => {
                                  if (mode === "create" && !isTimeSlotOccupied(dayIndex, time)) {
                                    handleCellClick(dayIndex, time)
                                  }
                                }}
                            >
                              {mode === "create" && !isTimeSlotOccupied(dayIndex, time) && (
                                  <div className="flex h-full items-center justify-center text-blue-500">
                                    <Plus className="h-4 w-4" />
                                  </div>
                              )}
                            </td>
                        )
                      }

                      const rowSpan = getCourseTimeSpan(course.startTime, course.endTime)

                      return (
                          <td key={dayIndex} className="relative border p-1 m-1" rowSpan={rowSpan}>
                            <div className={`${getTypeColor(course.type)} group p-1 rounded`}>
                              <div className="flex flex-col items-center justify-center text-center">
                                <div className={`mb-1 w-full rounded px-1 py-0.5 text-xs ${getTypeLabelColor(course.type)}`}>
                                  {course.type} - {course.startTime} - {course.endTime}
                                </div>
                                <div className="font-bold text-xs">{course.name}</div>
                                <div className="text-xs">{course.instructor}</div>
                                <div className="text-xs">{course.room}</div>
                              </div>

                              {mode === "update" && (
                                  <div className="absolute top-1 right-1 flex gap-1">
                                    <Button
                                        variant="ghost"
                                        size="icon"
                                        className="h-6 w-6 rounded-full bg-white p-1 hover:bg-gray-100"
                                        onClick={() => {
                                          setSelectedCourse(course)
                                          setIsEditDialogOpen(true)
                                        }}
                                    >
                                      <Edit className="h-4 w-4 text-blue-500" />
                                    </Button>
                                  </div>
                              )}

                              {mode === "delete" && (
                                  <div className="absolute top-1 right-1 flex gap-1  ">
                                    <Button
                                        variant="ghost"
                                        size="icon"
                                        className="h-6 w-6 rounded-full bg-white p-1 hover:bg-gray-100"
                                        onClick={() => {
                                          setSelectedCourse(course)
                                          setIsDeleteDialogOpen(true)
                                        }}
                                    >
                                      <Trash2 className="h-4 w-4 text-red-500" />
                                    </Button>
                                  </div>
                              )}
                            </div>
                          </td>
                      )
                    })}
                  </tr>
              )
            })}
            </tbody>
          </table>
        </div>

        <ScheduleLegend />
      </div>
  )
}
