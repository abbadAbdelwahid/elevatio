import { Tabs, TabsList, TabsTrigger } from "@/components/ui/tabs"

export function ScheduleTabs({ children }) {
  return (
    <Tabs defaultValue="schedule" className="w-full">
      <TabsList className="grid w-full grid-cols-2">
        <TabsTrigger value="schedule">Schedule</TabsTrigger>
        <TabsTrigger value="settings">Settings</TabsTrigger>
      </TabsList>
      {children}
    </Tabs>
  )
}
