import { CourseFilters } from "@/components/courses/course-filters"
import { CourseGrid } from "@/components/courses/course-grid"
import { SearchHeader } from "@/components/search-header"

export default function CoursesPage() {
    return (
        <div className="flex min-h-screen max-h-screen overflow-y-auto bg-amber-50">
            {/* Main content */}
            <div className="flex-1 p-8 ms-4">
                <SearchHeader />
                <CourseFilters />
                <CourseGrid />

            </div>
        </div>
    )
}
