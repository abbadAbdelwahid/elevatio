"use client"

import { Button } from "@/components/ui/button"
import {
  Dialog,
  DialogContent,
  DialogDescription,
  DialogFooter,
  DialogHeader,
  DialogTitle
} from "@/components/ui/dialog"
import {useEffect, useState} from "react";

export function DeleteCourseDialog({ isOpen, setIsOpen, selectedCourse, onDelete }) {
  if (!selectedCourse) return null

  return (
      <Dialog open={isOpen} onOpenChange={setIsOpen}>
        <DialogContent className="sm:max-w-[425px] bg-white border-0 rounded-3xl">
          <DialogHeader>
            <DialogTitle>Supprimer le cours</DialogTitle>
            <DialogDescription>
              Êtes-vous sûr de vouloir supprimer le cours{" "}
              <strong>{selectedCourse.name}</strong> ? Cette action est irréversible.
            </DialogDescription>
          </DialogHeader>

          <div className="text-sm text-muted-foreground px-2">
            {selectedCourse.instructor} – {selectedCourse.room} –{" "}
            {selectedCourse.startTime} - {selectedCourse.endTime}
          </div>

          <DialogFooter className="mt-4">
            <Button variant="outline" onClick={() => setIsOpen(false)}>
              Annuler
            </Button>
            <Button  className={'bg-red-600'} onClick={() => onDelete(selectedCourse.id)}>
              Supprimer
            </Button>
          </DialogFooter>
        </DialogContent>
      </Dialog>
  )
}
