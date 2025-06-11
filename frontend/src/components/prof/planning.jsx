"use client"

import { useEffect, useState, useMemo } from "react"
import { Card, CardContent } from "@/components/ui/card"
import { Button } from "@/components/ui/button"
import { ScheduleSettings } from "@/components/prof/schedule-settings"
import { ScheduleGrid } from "@/components/schedule/gestion-planning/schedule-grid"
import EditCourseDialog from "@/components/schedule/gestion-planning/edit-course-dialog"
import { DeleteCourseDialog } from "@/components/schedule/gestion-planning/delete-course-dialog"

export default function ScheduleManagement() {
  const [group, setGroup] = useState("GINF2")
  const [week, setWeek] = useState("")
  const [year, setYear] = useState("")
  const [mode, setMode] = useState("view")
  const [planningData, setPlanningData] = useState([])
  const [selectedCourse, setSelectedCourse] = useState(null)
  const [isEditDialogOpen, setIsEditDialogOpen] = useState(false)
  const [isDeleteDialogOpen, setIsDeleteDialogOpen] = useState(false)

  const currentKey = `${group}-${year}-${week}`
  const courses = planningData[currentKey] || []

  useEffect(() => {
    const today = new Date()
    const reference = new Date(2024, 10, 1) // 1er novembre 2024
    const diffInWeeks = Math.floor((today - reference) / (1000 * 60 * 60 * 24 * 7))
    const defaultWeek = Math.max(1, diffInWeeks + 1)
    if (!week) setWeek(defaultWeek.toString())
    if (!year) setYear(today.getFullYear().toString())
  }, [week, year])

  useEffect(() => {
    const fetchPlanning = async () => {
      if (!group || !year || !week) return

      try {
        const baseUrl = process.env.NEXT_PUBLIC_API_COURSE_URL
        const res = await fetch(`${baseUrl}/api/Schedule?group=${group}&year=${year}&week=${week}`)
        if (!res.ok) throw new Error("Erreur lors du chargement du planning")

        const data = await res.json()
        console.log(data.courses)
        const key = `${data.group}-${data.year}-${data.week}`
        setPlanningData((prev) => ({
          ...prev,
          [key]: data.courses || []
        }))
      } catch (err) {
        console.error("Erreur fetch planning:", err)
      }
    }

    fetchPlanning()
  }, [group, year, week])

  const days = useMemo(() => {
    if (!week || !year) return []
    const base = new Date(parseInt(year), 0, 1 + (parseInt(week) - 1) * 7)
    while (base.getDay() !== 1) base.setDate(base.getDate() + 1)

    return Array.from({ length: 6 }, (_, i) => {
      const date = new Date(base)
      date.setDate(base.getDate() + i)
      return {
        name: ["Lundi", "Mardi", "Mercredi", "Jeudi", "Vendredi", "Samedi"][i],
        date: date.toLocaleDateString("fr-FR", { day: "2-digit", month: "2-digit" })
      }
    })
  }, [week, year])

  const timeSlots = [
    "08:00", "08:30", "09:00", "09:30", "10:00", "10:30",
    "11:00", "11:30", "12:00", "12:30", "13:00", "13:30",
    "14:00", "14:30", "15:00", "15:30", "16:00", "16:30",
    "17:00", "17:30", "18:00"
  ]

  return (
      <div className="space-y-4">
        <div className="flex gap-4 w-full">
          <div className="w-1/4">
            <ScheduleSettings
                setGroup={setGroup}
                week={week}
                setWeek={setWeek}
                year={year}
                setYear={setYear}
            />
          </div>

          <div className="w-3/4">
            <Card className="overflow-hidden">
              <CardContent className="p-0">
                {/* Affichage du groupe dans l'en-tête de la carte */}
                <div className="p-4 border-b text-lg font-semibold text-gray-700">
                  Emploi du temps du groupe : <span className="text-blue-600">{group}</span>
                </div>

                <ScheduleGrid
                    days={days}
                    timeSlots={timeSlots}
                    courses={courses}
                    mode={mode}
                    setSelectedCourse={setSelectedCourse}
                    setIsEditDialogOpen={setIsEditDialogOpen}
                    setIsDeleteDialogOpen={setIsDeleteDialogOpen}
                />
              </CardContent>
            </Card>
          </div>
        </div>
      </div>
  )
}
