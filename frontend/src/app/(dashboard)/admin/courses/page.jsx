
import { SearchHeader } from "@/components/layout/search-header"
import {CoursesTable} from "@/components/admin/courses-table";



export default function CoursesPage() {
    return (
        <div className="flex min-h-screen max-h-screen overflow-y-auto bg-blue-50">
            {/* Main content */}
            <div className="flex-1 p-8 ms-4">
                <SearchHeader />
                <CoursesTable  />




            </div>
        </div>
    )
}
