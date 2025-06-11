
import { SearchHeader } from "@/components/layout/search-header"

import Planning from "@/components/prof/planning";




export default function CoursesPage() {
    return (
        <div className="flex min-h-screen max-h-screen overflow-y-auto bg-blue-50">
            {/* Main content */}
            <div className="flex-1 p-8 ms-4">
                <SearchHeader />
                <Planning />
            </div>
        </div>
    )
}
