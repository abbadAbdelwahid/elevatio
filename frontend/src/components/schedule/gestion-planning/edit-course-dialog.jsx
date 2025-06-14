"use client"

import { useState, useEffect } from "react"
import { Button } from "@/components/ui/button"
import { Input } from "@/components/ui/input"
import { Label } from "@/components/ui/label"
import { Select, SelectContent, SelectItem, SelectTrigger, SelectValue } from "@/components/ui/select"
import { Dialog, DialogContent, DialogDescription, DialogFooter, DialogHeader, DialogTitle } from "@/components/ui/dialog"

export default function EditCourseDialog({ isOpen, setIsOpen, selectedCourse, days, onSave,courses}) {
  console.log(courses)
  const [editedCourse, setEditedCourse] = useState(selectedCourse || {})
  const [isClient, setIsClient] = useState(false)

  useEffect(() => {
    setIsClient(true)
  }, [])

  useEffect(() => {
    setEditedCourse(selectedCourse || {})
  }, [selectedCourse])

  const handleSubmit = (e) => {
    e.preventDefault()
    onSave(editedCourse)
  }

  if (!isClient || !selectedCourse) return null

  return (
      <Dialog open={isOpen} onOpenChange={setIsOpen}>
        <DialogContent className="sm:max-w-[425px] bg-white bordr-0 rounded-3xl">
          <form onSubmit={handleSubmit}>
            <DialogHeader>
              <DialogTitle>Modifier le cours</DialogTitle>
              <DialogDescription>
                Modifiez les informations du cours.
              </DialogDescription>
            </DialogHeader>

            <div className="grid gap-4 py-4">
              {/* Nom */}
              <div className="grid grid-cols-4 items-center gap-4">
                <Label htmlFor="module" className="text-right">Module</Label>
                <select
                    id="module"
                    value={editedCourse.moduleId || ""}
                    onChange={(e) => setEditedCourse({ ...editedCourse, moduleId: parseInt(e.target.value) })}
                    className="col-span-3 border rounded-md p-2"
                >
                  <option value="">-- Sélectionner un module --</option>
                  {courses.map((mod) => (
                      <option key={mod.moduleId} value={mod.moduleId}>
                        {mod.moduleId}
                      </option>
                  ))}
                </select>
              </div>


              {/* Salle */}
              <div className="grid grid-cols-4 items-center gap-4">
                <Label htmlFor="room" className="text-right">Salle</Label>
                <Input
                    id="room"
                    value={editedCourse.room || ""}
                    onChange={(e) =>
                        setEditedCourse({ ...editedCourse, room: e.target.value })
                    }
                    className="col-span-3"
                />
              </div>

              {/* Jour */}
              <div className="grid grid-cols-4 items-center gap-4">
                <Label className="text-right">Jour</Label>
                <Select
                    value={editedCourse.day?.toString()}
                    onValueChange={(value) =>
                        setEditedCourse({ ...editedCourse, day: parseInt(value) })
                    }
                >
                  <SelectTrigger className="col-span-3">
                    <SelectValue placeholder="Sélectionner un jour" />
                  </SelectTrigger>
                  <SelectContent className={'bg-gray-100'}>
                    {days.map((day, index) => (
                        <SelectItem key={index} value={index.toString()}>
                          {day.name} ({day.date})
                        </SelectItem>
                    ))}
                  </SelectContent>
                </Select>
              </div>

              {/* Horaire */}
              <div className="grid grid-cols-4 items-center gap-4 ">
                <Label className="text-right">Horaire</Label>
                <div className="col-span-3 flex gap-2">
                  <Select
                      value={editedCourse.startTime}
                      onValueChange={(value) =>
                          setEditedCourse({ ...editedCourse, startTime: value })
                      }
                  >
                    <SelectTrigger>
                      <SelectValue placeholder="Début" />
                    </SelectTrigger>
                    <SelectContent className={'bg-gray-100'}>
                      {[
                        "08:00", "08:30", "09:00", "09:30", "10:00", "10:30",
                        "11:00", "11:30", "12:00", "12:30", "13:00", "13:30",
                        "14:00", "14:30", "15:00", "15:30", "16:00", "16:30",
                        "17:00", "17:30", "18:00"
                      ].map((time) => (
                          <SelectItem key={time} value={time}>{time}</SelectItem>
                      ))}
                    </SelectContent>
                  </Select>

                  <span className="mx-1">-</span>

                  <Select
                      value={editedCourse.endTime}
                      onValueChange={(value) =>
                          setEditedCourse({ ...editedCourse, endTime: value })
                      }
                  >
                    <SelectTrigger>
                      <SelectValue placeholder="Fin" />
                    </SelectTrigger>
                    <SelectContent className={'bg-gray-100'}>
                      {[
                        "08:30", "09:00", "09:30", "10:00", "10:30", "11:00",
                        "11:30", "12:00", "12:30", "13:00", "13:30", "14:00",
                        "14:30", "15:00", "15:30", "16:00", "16:30", "17:00",
                        "17:30", "18:00", "18:30"
                      ].map((time) => (
                          <SelectItem key={time} value={time}>{time}</SelectItem>
                      ))}
                    </SelectContent>
                  </Select>
                </div>
              </div>
            </div>

            <DialogFooter>
              <Button type="submit">Enregistrer</Button>
            </DialogFooter>
          </form>
        </DialogContent>
      </Dialog>
  )
}
