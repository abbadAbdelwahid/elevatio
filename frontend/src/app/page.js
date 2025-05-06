import Image from "next/image";
import Link from "next/link"

export default function Home() {
    return (
        <main className="min-h-screen">
            {/* Header/Navigation */}
            <header className="bg-white shadow-sm">
                <div className="container mx-auto px-4 py-4 flex items-center justify-between">
                    <div className="flex items-center gap-2">
                        <div className="w-8 h-8 bg-purple-800 rounded-md flex items-center justify-center">
                            <span className="text-white font-bold">Q</span>
                        </div>
                        <span className="font-bold text-purple-800">QualiTrack</span>
                    </div>

                    <nav className="hidden md:flex items-center gap-8">
                        <Link href="/" className="text-sm font-medium">
                            Home
                        </Link>
                        <Link href="/about" className="text-sm font-medium">
                            About us
                        </Link>
                        <Link href="/courses" className="text-sm font-medium">
                            Courses
                        </Link>
                        <Link href="/stories" className="text-sm font-medium">
                            Our Stories
                        </Link>
                    </nav>

                    <div className="flex items-center gap-3">
                        <Link href="/signup" className="text-sm font-medium">
                            Sign up
                        </Link>
                        <Link
                            href="/login"
                            className="bg-purple-800 hover:bg-purple-900 text-white rounded-full px-6 py-2 text-sm font-medium"
                        >
                            Sign in
                        </Link>
                    </div>
                </div>
            </header>

            {/* Hero Section */}
            <section className="bg-amber-50 py-16 relative overflow-hidden">
                <div className="container mx-auto px-4 grid md:grid-cols-2 gap-8 items-center">
                    <div className="space-y-6 z-10">
                        <div className="space-y-3">
                            <h2 className="text-4xl md:text-5xl font-bold text-gray-900">
                                Enhance the Quality of <br />
                                Higher Education
                            </h2>
                            <h2 className="text-4xl md:text-5xl font-bold text-orange-500">QualiTrack</h2>
                        </div>
                        <p className="text-gray-700 max-w-md">
                            A smart platform to manage, automate, and analyze training evaluations in universities and engineering schools.
                        </p>
                    </div>
                    <div className="relative z-10">
                        <Image
                            src="/images/acceuil/first.svg"
                            alt="Education illustration"
                            width={600}
                            height={400}
                            className="object-contain"
                        />
                    </div>

                    {/* Background patterns */}
                    <div className="absolute inset-0 opacity-10">
                        <div className="absolute top-20 left-10 w-8 h-8 border-2 border-gray-400 rotate-45"></div>
                        <div className="absolute bottom-20 left-1/4 w-12 h-12 rounded-full border-2 border-gray-400"></div>
                        <div className="absolute top-1/3 right-1/4 w-10 h-10 border-2 border-gray-400"></div>
                    </div>
                </div>
            </section>

            {/* Features Section - avec couleur orange seulement sur la moitié supérieure */}
            <section className="relative">
                {/* Première moitié avec fond orange */}
                <div className="bg-amber-50 h-1/2 absolute top-0 left-0 right-0"></div>

                {/* Contenu complet de la section */}
                <div className="relative py-16">
                    <div className="max-w-7xl mx-auto px-6">
                        <div className="bg-purple-900 rounded-xl py-12 px-8 grid md:grid-cols-3 gap-8 text-white">
                            {/* Block 1 */}
                            <div className="flex items-start gap-4">
                                <div className="flex-shrink-0 mt-4 bg-[#7151A3] p-3 rounded-xl">
                                    <img src="/images/acceuil/computer.svg" alt="Give Feedback" className="w-8 h-8" />
                                </div>
                                <div>
                                    <h2 className="text-lg font-semibold mb-2">Give Feedback Easily</h2>
                                    <p className="text-sm text-gray-200 font-roboto">
                                        Contrary to popular belief, Lorem Ipsum is not simply random text. It has roots in a BC,
                                        making it over 2000 years old.
                                    </p>
                                </div>
                            </div>

                            {/* Block 2 */}
                            <div className="flex items-start gap-4">
                                <div className="flex-shrink-0 mt-4 bg-[#7151A3] p-3 rounded-xl">
                                    <img src="/images/acceuil/note.svg" alt="Get Ready" className="w-8 h-8" />
                                </div>
                                <div>
                                    <h2 className="text-lg font-semibold mb-2">Get Ready For a Career</h2>
                                    <p className="text-sm text-gray-200 font-roboto">
                                        Students submit evaluations through simple and anonymous forms.
                                    </p>
                                </div>
                            </div>

                            {/* Block 3 */}
                            <div className="flex items-start gap-4">
                                <div className="flex-shrink-0 mt-4 bg-[#7151A3] p-3 rounded-xl">
                                    <img src="/images/acceuil/cert.svg" alt="Drive Excellence" className="w-8 h-8" />
                                </div>
                                <div>
                                    <h2 className="text-lg font-semibold mb-2">Drive Institutional Excellence</h2>
                                    <p className="text-sm text-gray-200 font-roboto">
                                        Administrators use statistics and reports to enhance programs.
                                    </p>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </section>

            {/* Platform Preview Section */}
            <section className="py-16 bg-amber-50">
                <div className="container mx-auto px-4 text-center">
                    <h2 className="text-3xl font-bold mb-2">Plateform Preview</h2>
                    <p className="text-gray-500 mb-8">Lorem ipsum is simply dummy text of the printing.</p>

                    <div className="max-w-4xl mx-auto rounded-lg overflow-hidden shadow-lg">
                        <Image
                            src="/placeholder.svg?height=400&width=800"
                            alt="Platform preview"
                            width={800}
                            height={400}
                            className="w-full"
                        />
                    </div>
                </div>
            </section>

            {/* Smart Evaluation Section */}
            <section className="py-16 bg-white">
                <div className="container mx-auto px-4">
                    <div className="grid md:grid-cols-2 gap-8 items-center">
                        <div className="relative">
                            <Image
                                src="/placeholder.svg?height=400&width=500"
                                alt="Smart evaluation illustration"
                                width={500}
                                height={400}
                                className="object-contain"
                            />
                            <div className="absolute -bottom-10 -left-10 w-20 h-20 text-purple-600">
                                <svg viewBox="0 0 100 100" xmlns="http://www.w3.org/2000/svg" fill="currentColor">
                                    <path d="M30,10 Q10,50 30,90 Q50,70 70,90 Q90,50 70,10 Q50,30 30,10 Z" />
                                </svg>
                            </div>
                        </div>

                        <div>
                            <h2 className="text-3xl font-bold mb-6">
                                <span>Smart & Transparent</span>
                                <br />
                                <span className="text-orange-500">Evaluation</span> Experience
                            </h2>

                            <div className="space-y-4">
                                <div className="flex gap-4">
                                    <div className="bg-pink-100 p-2 rounded-lg">
                                        <svg
                                            xmlns="http://www.w3.org/2000/svg"
                                            width="24"
                                            height="24"
                                            viewBox="0 0 24 24"
                                            fill="none"
                                            stroke="currentColor"
                                            strokeWidth="2"
                                            strokeLinecap="round"
                                            strokeLinejoin="round"
                                            className="text-pink-600"
                                        >
                                            <path d="M16 21v-2a4 4 0 0 0-4-4H6a4 4 0 0 0-4 4v2" />
                                            <circle cx="9" cy="7" r="4" />
                                            <path d="M22 21v-2a4 4 0 0 0-3-3.87" />
                                            <path d="M16 3.13a4 4 0 0 1 0 7.75" />
                                        </svg>
                                    </div>
                                    <div>
                                        <h3 className="font-semibold">Easily Accessible</h3>
                                        <p className="text-sm text-gray-600">Existing tool for busy Coordinators With QualiTrack</p>
                                    </div>
                                </div>

                                <div className="flex gap-4">
                                    <div className="bg-pink-100 p-2 rounded-lg">
                                        <svg
                                            xmlns="http://www.w3.org/2000/svg"
                                            width="24"
                                            height="24"
                                            viewBox="0 0 24 24"
                                            fill="none"
                                            stroke="currentColor"
                                            strokeWidth="2"
                                            strokeLinecap="round"
                                            strokeLinejoin="round"
                                            className="text-pink-600"
                                        >
                                            <path d="M2 3h6a4 4 0 0 1 4 4v14a3 3 0 0 0-3-3H2z" />
                                            <path d="M22 3h-6a4 4 0 0 0-4 4v14a3 3 0 0 1 3-3h7z" />
                                        </svg>
                                    </div>
                                    <div>
                                        <h3 className="font-semibold">Fun learning page</h3>
                                        <p className="text-sm text-gray-600">Powerful access to reports and analytics</p>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </section>

            {/* Testimonials Section */}
            <section className="py-16 bg-white">
                <div className="container mx-auto px-4 text-center">
                    <h2 className="text-3xl font-bold mb-2">Testimonials</h2>
                    <p className="text-gray-500 mb-12">Lorem ipsum is simply dummy text of the printing.</p>

                    <div className="grid md:grid-cols-3 gap-6">
                        <div className="bg-white p-6 rounded-lg shadow-md">
                            <p className="text-gray-600 mb-4">
                                "QualiTrack helped us streamline our evaluation process and made it much more efficient."
                            </p>
                            <div className="flex items-center">
                                <div className="w-10 h-10 bg-gray-200 rounded-full mr-3"></div>
                                <div className="text-left">
                                    <p className="font-semibold">Dr. Mark</p>
                                    <p className="text-sm text-gray-500">University Professor</p>
                                </div>
                            </div>
                        </div>

                        <div className="bg-white p-6 rounded-lg shadow-md">
                            <p className="text-gray-600 mb-4">
                                "The analytics provided by QualiTrack have been invaluable for improving our courses."
                            </p>
                            <div className="flex items-center">
                                <div className="w-10 h-10 bg-gray-200 rounded-full mr-3"></div>
                                <div className="text-left">
                                    <p className="font-semibold">Jane</p>
                                    <p className="text-sm text-gray-500">Department Coordinator</p>
                                </div>
                            </div>
                        </div>

                        <div className="bg-white p-6 rounded-lg shadow-md">
                            <p className="text-gray-600 mb-4">
                                "As a student, I appreciate how easy it is to provide feedback through QualiTrack."
                            </p>
                            <div className="flex items-center">
                                <div className="w-10 h-10 bg-gray-200 rounded-full mr-3"></div>
                                <div className="text-left">
                                    <p className="font-semibold">Jessica Williams</p>
                                    <p className="text-sm text-gray-500">Engineering Student</p>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div className="flex justify-center mt-8 gap-1">
                        <div className="w-8 h-2 bg-purple-800 rounded-full"></div>
                        <div className="w-2 h-2 bg-gray-300 rounded-full"></div>
                        <div className="w-2 h-2 bg-gray-300 rounded-full"></div>
                    </div>
                </div>
            </section>

            {/* About Section */}
            <section className="py-16 bg-white">
                <div className="container mx-auto px-4 text-center">
                    <h2 className="text-3xl font-bold mb-2">About EvalForma</h2>
                    <p className="text-gray-500 mb-12">Lorem ipsum is simply dummy text of the printing.</p>
                </div>
            </section>

            {/* Benefits Section */}
            <section className="py-16 bg-white">
                <div className="container mx-auto px-4">
                    <div className="grid md:grid-cols-2 gap-8 items-center">
                        <div>
                            <Image
                                src="/placeholder.svg?height=400&width=500"
                                alt="Education professionals"
                                width={500}
                                height={400}
                                className="rounded-lg"
                            />
                        </div>

                        <div>
                            <h2 className="text-3xl font-bold mb-6">
                                <span>Benefit From Our Online</span>
                                <br />
                                <span>Learning Expertise Earn</span>
                                <br />
                                <span className="text-pink-500">Professional</span>
                            </h2>

                            <div className="grid grid-cols-2 gap-6 mb-8">
                                <div>
                                    <p className="text-sm text-gray-600 mb-4">
                                        Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore
                                        et dolore magna aliqua.
                                    </p>
                                    <div className="flex items-center gap-2">
                                        <svg
                                            xmlns="http://www.w3.org/2000/svg"
                                            width="20"
                                            height="20"
                                            viewBox="0 0 24 24"
                                            fill="none"
                                            stroke="currentColor"
                                            strokeWidth="2"
                                            strokeLinecap="round"
                                            strokeLinejoin="round"
                                            className="text-green-500"
                                        >
                                            <polyline points="20 6 9 17 4 12" />
                                        </svg>
                                        <p className="text-sm">Professional certification</p>
                                    </div>
                                </div>

                                <div>
                                    <p className="text-sm text-gray-600 mb-4">
                                        Consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.
                                    </p>
                                    <div className="flex items-center gap-2">
                                        <svg
                                            xmlns="http://www.w3.org/2000/svg"
                                            width="20"
                                            height="20"
                                            viewBox="0 0 24 24"
                                            fill="none"
                                            stroke="currentColor"
                                            strokeWidth="2"
                                            strokeLinecap="round"
                                            strokeLinejoin="round"
                                            className="text-green-500"
                                        >
                                            <polyline points="20 6 9 17 4 12" />
                                        </svg>
                                        <p className="text-sm">Flexible learning schedule</p>
                                    </div>
                                </div>
                            </div>

                            <button className="bg-teal-500 hover:bg-teal-600 text-white rounded-full px-6 py-2 flex items-center">
                                Learn More
                                <svg
                                    xmlns="http://www.w3.org/2000/svg"
                                    width="16"
                                    height="16"
                                    viewBox="0 0 24 24"
                                    fill="none"
                                    stroke="currentColor"
                                    strokeWidth="2"
                                    strokeLinecap="round"
                                    strokeLinejoin="round"
                                    className="ml-2"
                                >
                                    <polyline points="9 18 15 12 9 6" />
                                </svg>
                            </button>
                        </div>
                    </div>
                </div>
            </section>

            {/* Footer */}
            <footer className="bg-purple-900 text-white py-12">
                <div className="container mx-auto px-4">
                    <div className="text-center">
                        <p>© 2023 QualiTrack. All rights reserved.</p>
                    </div>
                </div>
            </footer>
        </main>
    )
}
