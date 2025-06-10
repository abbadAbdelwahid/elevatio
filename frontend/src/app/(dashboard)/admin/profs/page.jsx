'use client'
import { SearchHeader } from "@/components/layout/search-header"
import {CornerUpLeft} from "lucide-react";
import {CoursesTable} from "@/components/admin/courses-table";

import ProfsTable from "@/components/admin/prof-table";
import QuestionnairesManager from "@/components/admin/questionnaire-admin";
import ScheduleManagement from "@/components/schedule/schedule-management";




export default function Professors() {
    return (
        <div className="flex min-h-screen max-h-screen overflow-y-auto bg-blue-50">
            {/* Main content */}
            <div className="flex-1 p-8 ms-4">
                <SearchHeader />
                <ProfsTable />




            </div>
        </div>
    )
}
