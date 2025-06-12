'use client'

import { useEffect, useState } from "react"
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
    const [moduleNames, setModuleNames] = useState({})

    useEffect(() => {
        const fetchModuleNames = async () => {
            const uniqueIds = [...new Set(courses.map((c) => c.moduleId))]
            const fetchedNames = {}

            await Promise.all(
                uniqueIds.map(async (id) => {
                    try {
                        const baseUrl = process.env.NEXT_PUBLIC_API_COURSE_URL
                        const res = await fetch(`${baseUrl}/api/Module/${id}`)
                        if (res.ok) {
                            const data = await res.json()
                            fetchedNames[id] = data.moduleName
                        } else {
                            fetchedNames[id] = `Module ${id}`
                        }
                    } catch {
                        fetchedNames[id] = `Module ${id}`
                    }
                })
            )

            setModuleNames(fetchedNames)
        }

        if (courses.length > 0) {
            fetchModuleNames()
        }
    }, [courses])

    const getCourseAtTime = (dayName, time) => {
        return courses.find((course) => course.day === dayName && course.start === time)
    }

    const getCourseTimeSpan = (start, end) => {
        const startTime = timeSlots.indexOf(start)
        const endTime = timeSlots.indexOf(end)
        return endTime - startTime
    }

    // Fonction pour déterminer le type de module et la couleur de fond
    const getModuleBackgroundColor = (moduleId) => {
        if (moduleId < 20) {
            return "bg-purple-200"; // Cours
        } else if (moduleId < 40) {
            return "bg-yellow-200"; // TD
        } else {
            return "bg-green-200"; // TP
        }
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
                            days.some((day) => {
                                const prevCourse = getCourseAtTime(day.name, timeSlots[timeIndex - 1])
                                return prevCourse && prevCourse.end !== time
                            })

                        if (skipRendering) return null

                        return (
                            <tr key={timeIndex} className="h-7">
                                <td className="border bg-gray-100 p-1 text-center text-xs font-medium">{time}</td>
                                {days.map((day, dayIndex) => {
                                    const course = getCourseAtTime(day.name, time)

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

                                    const rowSpan = getCourseTimeSpan(course.start, course.end)
                                    const moduleColor = getModuleBackgroundColor(course.moduleId)

                                    return (
                                        <td key={dayIndex} className="relative border p-1 m-1" rowSpan={rowSpan}>
                                            <div className={`${moduleColor} border border-gray-300 group p-1 rounded`}>
                                                <div className="flex flex-col items-center justify-center text-center">
                                                    <div className="mb-1 w-full rounded px-1 py-0.5 text-xs bg-gray-300">
                                                        {course.start} - {course.end}
                                                    </div>
                                                    <div className="font-bold text-xs">
                                                        {moduleNames[course.moduleId] || `Module ${course.moduleId}`}
                                                    </div>
                                                    <div className="text-xs">{course.location}</div>
                                                    <div className="text-xs">{course.teacherFullName}</div>
                                                    <div className="text-xs mt-1">{course.moduleId < 20 ? 'Cours' : course.moduleId < 40 ? 'TD' : 'TP'}</div>
                                                </div>
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
