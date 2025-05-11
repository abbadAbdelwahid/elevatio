"use client"

import { Button } from "@/components/ui/button"
import { Plus, Edit, Save, X } from "lucide-react"

export function ScheduleHeader({ mode, toggleEditMode, createNewSchedule, handleCreateCourse, selectedCell }) {
  return (
    <div className="flex justify-between">
      <h2 className="text-xl font-bold">Planning Management</h2>
      <div className="flex gap-2">
        {mode === "view" ? (
          <>
            <Button className="bg-green-600 hover:bg-green-700" onClick={createNewSchedule}>
              <Plus className="mr-2 h-4 w-4" /> Create New
            </Button>
            <Button className="bg-blue-600 hover:bg-blue-700" onClick={toggleEditMode}>
              <Edit className="mr-2 h-4 w-4" /> Edit
            </Button>
          </>
        ) : (
          <>
            <Button className="bg-green-600 hover:bg-green-700" onClick={handleCreateCourse} disabled={!selectedCell}>
              <Save className="mr-2 h-4 w-4" /> Save
            </Button>
            <Button variant="outline" onClick={toggleEditMode}>
              <X className="mr-2 h-4 w-4" /> Cancel
            </Button>
          </>
        )}
      </div>
    </div>
  )
}
