
import { SearchHeader } from "@/components/layout/search-header"
// ✅ bon si tu as "export default function AbsencePage"
import AbsencePage from "@/components/prof/student-list"





export default function CoursesPage() {
    return (
        <div className="flex min-h-screen max-h-screen overflow-y-auto bg-blue-50">
            {/* Main content */}
            <div className="flex-1 p-8 ms-4">
                <SearchHeader />
                {/*<AbsencePage />*/}




            </div>
        </div>
    )
}
