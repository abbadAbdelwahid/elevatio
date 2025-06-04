"use client"

import { useState } from "react"
import { ChevronLeft, ChevronRight } from "lucide-react"
import { Button } from "@/components/ui/button"
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card"

export function Calendar() {
    const [currentDate, setCurrentDate] = useState(new Date())
    const [currentMonth, setCurrentMonth] = useState(new Date().getMonth())
    const [currentYear, setCurrentYear] = useState(new Date().getFullYear())

    const months = [
        "January", "February", "March", "April", "May", "June",
        "July", "August", "September", "October", "November", "December"
    ]

    const weekdays = ["Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat"]

    const getDaysInMonth = (month, year) => {
        const date = new Date(year, month, 1)
        const days = []

        // Ajouter les jours du mois précédent
        const firstDay = date.getDay()
        const prevMonthLastDay = new Date(year, month, 0).getDate()
        for(let i = firstDay - 1; i >= 0; i--) {
            days.push({ date: prevMonthLastDay - i, month: "prev" })
        }

        // Ajouter les jours du mois courant
        const lastDay = new Date(year, month + 1, 0).getDate()
        for(let i = 1; i <= lastDay; i++) {
            days.push({
                date: i,
                month: "current",
                isToday: i === new Date().getDate() &&
                    month === new Date().getMonth() &&
                    year === new Date().getFullYear()
            })
        }

        // Ajouter les jours du mois suivant
        const totalCells = days.length > 35 ? 42 : 35
        const nextDays = totalCells - days.length
        for(let i = 1; i <= nextDays; i++) {
            days.push({ date: i, month: "next" })
        }

        return days
    }

    const handlePrevMonth = () => {
        const newMonth = currentMonth === 0 ? 11 : currentMonth - 1
        const newYear = currentMonth === 0 ? currentYear - 1 : currentYear
        setCurrentMonth(newMonth)
        setCurrentYear(newYear)
    }

    const handleNextMonth = () => {
        const newMonth = currentMonth === 11 ? 0 : currentMonth + 1
        const newYear = currentMonth === 11 ? currentYear + 1 : currentYear
        setCurrentMonth(newMonth)
        setCurrentYear(newYear)
    }

    const calendarDays = getDaysInMonth(currentMonth, currentYear)

    return (
        <Card className="bg-white shadow-lg rounded-xl p-4 border-0">
            <CardHeader className="flex flex-col items-center justify-between pb-2">
                <div className="flex items-center justify-between w-full mb-2">
                    <Button
                        variant="ghost"
                        size="icon"
                        className="h-7 w-7 rounded-full bg-[#ffe4e6] text-black hover:bg-[#fecdd3]"
                        onClick={handlePrevMonth}
                    >
                        <ChevronLeft className="h-4 w-4" />
                    </Button>
                    <div className="text-sm font-semibold text-gray-800">
                        {months[currentMonth]} {currentYear}
                    </div>
                    <Button
                        variant="ghost"
                        size="icon"
                        className="h-7 w-7 rounded-full bg-gray-200 text-gray-600 hover:bg-gray-300"
                        onClick={handleNextMonth}
                    >
                        <ChevronRight className="h-4 w-4" />
                    </Button>
                </div>
            </CardHeader>

            <CardContent>
                {/* Weekday headers */}
                <div className="grid grid-cols-7 gap-1 text-center mb-2">
                    {weekdays.map((day) => (
                        <div
                            key={day}
                            className="text-[13px] font-medium px-2 py-2 rounded-lg bg-gray-100 text-gray-600"
                        >
                            {day}
                        </div>
                    ))}
                </div>

                {/* Calendar grid */}
                <div className="grid grid-cols-7 gap-1 text-center text-sm">
                    {calendarDays.map((day, index) => (
                        <div
                            key={index}
                            className={`flex h-9 items-center justify-center rounded-full transition-all
            ${day.month !== "current" ? "text-gray-400" : "text-gray-900"}
            ${day.isToday ? "bg-[#16a34a] text-white font-semibold" : ""}
            ${!day.isToday && day.hasEvent ? "bg-green-100 text-green-900" : ""}
          `}
                        >
                            {day.date}
                        </div>
                    ))}
                </div>
            </CardContent>
        </Card>

    )
}