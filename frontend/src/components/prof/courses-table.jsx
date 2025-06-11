"use client"

import { useState } from "react"
import { Input } from "@/components/ui/input"
import { Label } from "@/components/ui/label"
import { Button } from "@/components/ui/button"
import {
    Table,
    TableBody,
    TableCell,
    TableHead,
    TableHeader,
    TableRow,
} from "@/components/ui/table"
import {
    Dialog,
    DialogContent,
    DialogHeader,
    DialogTitle,
} from "@/components/ui/dialog"

const initialCourses = [
    {
        id: 1,
        course: "C++",
        duration: 6,
        status: "in progress",
        description: "Intro to object-oriented programming",
        filiere: "GINF2",
        evaluation: 3.3,
    },
    {
        id: 2,
        course: "JavaScript",
        duration: 4,
        status: "termine",
        description: "Web fundamentals and async programming",
        filiere: "GINF1",
        evaluation: 4.6,
    },
]

export function CoursesTable() {
    const [courses, setCourses] = useState(initialCourses)

    const handleDescriptionChange = (id, newDesc) => {
        setCourses((prev) =>
            prev.map((c) => (c.id === id ? { ...c, description: newDesc } : c))
        )
    }

    return (
        <div className="rounded-lg border border-gray-200 bg-white shadow-sm">
            <Table>
                <TableHeader className="bg-[#f8f9fa]">
                    <TableRow className="border-b border-gray-200">
                        {["Course", "Duration", "Status", "Description", "Filiere", "Evaluation"].map(
                            (header) => (
                                <TableHead
                                    key={header}
                                    className="px-4 py-3 text-sm font-medium text-gray-700"
                                >
                                    {header}
                                </TableHead>
                            )
                        )}
                    </TableRow>
                </TableHeader>

                <TableBody>
                    {courses.map((course) => (
                        <TableRow key={course.id} className="border-b border-gray-100 hover:bg-gray-50">
                            <TableCell className="px-4 py-3 text-sm font-medium text-gray-900">
                                {course.course}
                            </TableCell>
                            <TableCell className="px-4 py-3 text-sm text-gray-600">
                                {course.duration} weeks
                            </TableCell>
                            <TableCell className="px-4 py-3">
                <span
                    className={`inline-flex items-center rounded-full px-2.5 py-0.5 text-xs font-medium ${
                        course.status === "in progress"
                            ? "bg-yellow-100 text-yellow-800"
                            : "bg-green-100 text-green-800"
                    }`}
                >
                  {course.status}
                </span>
                            </TableCell>
                            <TableCell className="px-4 py-3 text-sm text-gray-600">
                                <Input
                                    type="text"
                                    value={course.description}
                                    onChange={(e) => handleDescriptionChange(course.id, e.target.value)}
                                />
                            </TableCell>
                            <TableCell className="px-4 py-3 text-sm text-gray-600">
                                {course.filiere}
                            </TableCell>
                            <TableCell className="px-4 py-3 text-sm font-medium text-blue-600">
                                {course.evaluation}/5
                            </TableCell>
                        </TableRow>
                    ))}
                </TableBody>
            </Table>
        </div>
    )
}
