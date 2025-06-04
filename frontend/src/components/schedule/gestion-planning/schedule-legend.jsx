export function ScheduleLegend() {
  return (
    <div className="flex flex-wrap gap-2 border-t bg-gray-50 p-2">
      <div className="text-sm font-medium">Légende des en-têtes :</div>
      <div className="flex items-center gap-1">
        <div className="h-4 w-4 rounded bg-purple-200"></div>
        <span className="text-xs">Cours</span>
      </div>
      <div className="flex items-center gap-1">
        <div className="h-4 w-4 rounded bg-yellow-200"></div>
        <span className="text-xs">TD</span>
      </div>
      <div className="flex items-center gap-1">
        <div className="h-4 w-4 rounded bg-green-200"></div>
        <span className="text-xs">TP</span>
      </div>
      <div className="flex items-center gap-1">
        <div className="h-4 w-4 rounded bg-pink-200"></div>
        <span className="text-xs">DG</span>
      </div>
    </div>
  )
}
