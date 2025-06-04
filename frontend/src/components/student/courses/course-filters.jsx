'use client'

export function CourseFilters({ activeFilter, setActiveFilter }) {
    const filters = ["All", "Recent", "Evaluated", "NotEvaluated"]

    return (
        <div className="mb-8 flex flex-wrap gap-2">
            {filters.map((filter) => (
                <button
                    key={filter}
                    className={`rounded-full px-6 py-2 text-sm font-medium transition-colors ${
                        activeFilter === filter ? "bg-[#7e57c2] text-white" : "bg-white text-gray-700 hover:bg-gray-100"
                    }`}
                    onClick={() => setActiveFilter(filter)}
                >
                    {filter}
                </button>
            ))}
        </div>
    )
}
