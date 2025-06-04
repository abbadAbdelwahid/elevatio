"use client"

import { Button } from "@/components/ui/button"
import { Input } from "@/components/ui/input"
import { Label } from "@/components/ui/label"
import { Select, SelectContent, SelectItem, SelectTrigger, SelectValue } from "@/components/ui/select"

export function CourseForm({ newCourse, setNewCourse, handleCreateCourse, setSelectedCell, timeSlots }) {
  return (
    <div className="border-b bg-blue-50 p-4">
      <h3 className="mb-4 text-lg font-medium">Add Course</h3>
      <div className="grid grid-cols-1 gap-4 md:grid-cols-3">
        <div>
          <Label htmlFor="name">Course Name</Label>
          <Input
            id="name"
            value={newCourse.name}
            onChange={(e) => setNewCourse({ ...newCourse, name: e.target.value })}
            placeholder="e.g. SECURITE DES SYSTEMES"
          />
        </div>
        <div>
          <Label htmlFor="instructor">Instructor</Label>
          <Input
            id="instructor"
            value={newCourse.instructor}
            onChange={(e) => setNewCourse({ ...newCourse, instructor: e.target.value })}
            placeholder="e.g. EL ALAMI"
          />
        </div>
        <div>
          <Label htmlFor="room">Room</Label>
          <Input
            id="room"
            value={newCourse.room}
            onChange={(e) => setNewCourse({ ...newCourse, room: e.target.value })}
            placeholder="e.g. SALLE B2"
          />
        </div>
        <div>
          <Label htmlFor="type">Type</Label>
          <Select value={newCourse.type} onValueChange={(value) => setNewCourse({ ...newCourse, type: value })}>
            <SelectTrigger>
              <SelectValue placeholder="Select type" />
            </SelectTrigger>
            <SelectContent>
              <SelectItem value="CM">CM (Cours)</SelectItem>
              <SelectItem value="TD">TD (Travaux Dirigés)</SelectItem>
              <SelectItem value="TP">TP (Travaux Pratiques)</SelectItem>
              <SelectItem value="DG">DG</SelectItem>
            </SelectContent>
          </Select>
        </div>
        <div>
          <Label htmlFor="startTime">Start Time</Label>
          <Select
            value={newCourse.startTime}
            onValueChange={(value) => setNewCourse({ ...newCourse, startTime: value })}
          >
            <SelectTrigger>
              <SelectValue placeholder="Select start time" />
            </SelectTrigger>
            <SelectContent>
              {timeSlots.slice(0, -1).map((time) => (
                <SelectItem key={time} value={time}>
                  {time}
                </SelectItem>
              ))}
            </SelectContent>
          </Select>
        </div>
        <div>
          <Label htmlFor="endTime">End Time</Label>
          <Select value={newCourse.endTime} onValueChange={(value) => setNewCourse({ ...newCourse, endTime: value })}>
            <SelectTrigger>
              <SelectValue placeholder="Select end time" />
            </SelectTrigger>
            <SelectContent>
              {timeSlots.slice(1).map((time) => (
                <SelectItem key={time} value={time}>
                  {time}
                </SelectItem>
              ))}
            </SelectContent>
          </Select>
        </div>
      </div>
      <div className="mt-4 flex justify-end space-x-2">
        <Button variant="outline" onClick={() => setSelectedCell(null)}>
          Cancel
        </Button>
        <Button onClick={handleCreateCourse}>Add Course</Button>
      </div>
    </div>
  )
}
