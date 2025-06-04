"use client"

import { Card, CardContent } from "@/components/ui/card"
import { Button } from "@/components/ui/button"
import { Input } from "@/components/ui/input"
import { ChevronLeft, ChevronRight } from "lucide-react"
import { useEffect } from "react"

export function ScheduleSettings({ group, setGroup, week, setWeek, year, setYear }) {
  // Fonction pour calculer la semaine scolaire actuelle (à partir du 1er septembre)
  useEffect(() => {
    if (!week) {
      const today = new Date()
      const start = new Date(today.getFullYear(), 8, 1) // mois 8 = septembre (0-indexed)
      const diff = Math.floor((today - start) / (1000 * 60 * 60 * 24 * 7))
      const currentSchoolWeek = Math.max(1, diff + 1)
      setWeek(currentSchoolWeek.toString())
    }

    if (!year) {
      setYear(new Date().getFullYear().toString())
    }
  }, [week, year, setWeek, setYear])

  const handlePrevWeek = () => {
    setWeek((prev) => {
      const newWeek = Math.max(1, parseInt(prev) - 1)
      return newWeek.toString()
    })
  }

  const handleNextWeek = () => {
    setWeek((prev) => {
      const newWeek = Math.min(52, parseInt(prev) + 1)
      return newWeek.toString()
    })
  }

  return (
      <Card>
        <CardContent className="p-4 pt-6">
          <div className="space-y-4">
            <div>
              <label className="mb-2 block text-sm font-medium">Groupe</label>
              <select
                  className="w-full rounded-md border border-gray-300 p-2"
                  value={group}
                  onChange={(e) => setGroup(e.target.value)}
              >
                <option value="GINF2">GINF2</option>
                <option value="GINF1">GINF1</option>
                <option value="GSTR">GINF3</option>
              </select>
            </div>

            <div>
              <label className="mb-2 block text-sm font-medium">Semaine</label>
              <div className="flex items-center gap-2">
                <Button variant="ghost" size="icon" onClick={handlePrevWeek}>
                  <ChevronLeft className="h-4 w-4" />
                </Button>
                <Input
                    type="number"
                    min="1"
                    max="52"
                    value={week}
                    onChange={(e) => {
                      const value = Math.max(1, Math.min(52, parseInt(e.target.value)))
                      setWeek(value.toString())
                    }}
                    className="w-full text-center"
                />
                <Button variant="ghost" size="icon" onClick={handleNextWeek}>
                  <ChevronRight className="h-4 w-4" />
                </Button>
              </div>
            </div>

            <div>
              <label className="mb-2 block text-sm font-medium">Année</label>
              <Input
                  type="number"
                  value={year}
                  onChange={(e) => setYear(e.target.value)}
                  className="w-full"
              />
            </div>

            <Button className="w-full bg-blue-600 hover:bg-blue-700">Valider</Button>
          </div>
        </CardContent>
      </Card>
  )
}
