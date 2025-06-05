import Image from "next/image";
import Link from "next/link"
import ImageCarousel from "@/components/layout/carouselComponent";
import NavComponent from "@/components/layout/navComponent";


export default function Home() {
    return (
        <main className="min-h-screen">
            <NavComponent />

            {/* Hero Section */}
            <section className="bg-amber-50 py-16 relative overflow-hidden">
                <div className="container mx-auto px-4 grid md:grid-cols-2 gap-8 items-center">
                    <div className="space-y-6 z-10">
                        <div className="space-y-3">
                            <h2 className="text-4xl md:text-5xl font-bold text-gray-900">
                                Abbad is goinig to deploy our APP <br />
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
                        <div className="bg-[#4D2C5E] rounded-xl py-12 px-8 grid md:grid-cols-3 gap-8 text-white">
                            {/* Block 1 */}
                            <div className="flex items-start gap-4">
                                <div className="flex-shrink-0 mt-4 bg-[#7151A3] p-3 rounded-xl">
                                    <Image
                                        src="/images/acceuil/computer.svg"
                                        alt="Give Feedback"
                                        width={32}
                                        height={32}
                                        className="w-8 h-8"
                                    />
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
                                    <Image
                                        src="/images/acceuil/note.svg"
                                        alt="Get Ready"
                                        width={32}
                                        height={32}
                                        className="w-8 h-8"
                                    />
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
                                    <Image
                                        src="/images/acceuil/cert.svg"
                                        alt="Drive Excellence"
                                        width={32}
                                        height={32}
                                        className="w-8 h-8"
                                    />
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
            <section className="py-16 ">
                <div className="container mx-auto px-4 text-center">
                    <h2 className="text-3xl font-bold mb-2">Plateform Preview hhh</h2>
                    <p className="text-gray-500 mb-8">Lorem ipsum is simply dummy text of the printing.</p>

                    <ImageCarousel />

                </div>
            </section>

            {/* Smart Evaluation Section */}
            <section className="  bg-amber-50">
                <div className="container mx-auto">
                    <div className="grid md:grid-cols-2  items-center">
                        <div className="relative flex justify-center ">
                            <Image
                                src="/images/acceuil/second.svg"
                                alt="Smart evaluation illustration"
                                width={300}
                                height={200}
                                className="object-contain"
                            />

                        </div>

                        <div>
                            <h2 className="text-3xl font-bold mb-8">
                                <span>Smart & Transparent</span>
                                <br />
                                <span className="text-orange-500">Evaluation</span> Experience
                            </h2>

                            <div className="space-y-4">
                                <div className="flex gap-4">
                                    <div className="bg-[#4D2C5E] p-2 rounded-lg">

                                        <Image
                                            src="/images/acceuil/cube.svg"
                                            width={24}  // 24px = w-6 (tailwind)
                                            height={24} // 24px = h-6
                                            alt="Icône cube"

                                            className="w-6 h-6"
                                            priority // Si l'image est visible immédiatement au chargement
                                        />
                                    </div>
                                    <div>
                                        <h3 className="font-semibold">Easily Accessible</h3>
                                        <p className="text-sm text-gray-600">Existing tool for busy Coordinators With QualiTrack</p>
                                    </div>
                                </div>

                                <div className="flex gap-4">
                                    <div className="bg-[#4D2C5E] p-2 rounded-lg">
                                        <Image
                                            src="/images/acceuil/heart.svg"
                                            width={24}
                                            height={24}
                                            alt="Icône cœur"
                                            className="w-6 h-6"
                                        />
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
            <section className="py-20 bg-white">
                <div className="container  px-4 mx-auto text-center">
                    <h2 className="text-3xl font-bold mb-2">Testimonials</h2>
                    <p className="text-gray-500 mb-12">Lorem ipsum is simply dummy text of the printing.</p>

                    <div className="grid md:grid-cols-3 gap-4 ">
                        {/* Premier témoignage */}
                        <div className="bg-white p-6 rounded-lg shadow-md">
                            <p className="text-gray-600 mb-4 italic">
                                &quot;I received detailed feedback on my teaching. Ewthoma is a game changer.&quot;
                            </p>
                            <div className="flex items-center justify-start">
                                <div className="w-10 h-10 bg-gray-200 rounded-full mr-3"></div>
                                <div className="text-left">
                                    <p className="font-semibold">M. Said</p>
                                    <p className="text-sm text-gray-500">Professor</p>
                                </div>
                            </div>
                        </div>

                        {/* Deuxième témoignage */}
                        <div className="bg-white p-6 rounded-lg shadow-md">
                            <p className="text-gray-600 mb-4 italic">
                                &quot;Complete account of the system and expound the actual Contrary to popular belief, Lorem Ipsum is not simply random text. It has roots&quot;
                            </p>
                            <div className="flex items-center justify-start">
                                <div className="w-10 h-10 bg-gray-200 rounded-full mr-3"></div>
                                <div className="text-left">
                                    <p className="font-semibold">Zhebe</p>
                                    <p className="text-sm text-gray-500">Sinhane University Student</p>
                                </div>
                            </div>
                        </div>

                        {/* Troisième témoignage */}
                        <div className="bg-white p-6 rounded-lg shadow-md">
                            <p className="text-gray-600 mb-4 italic">
                                &quot;There are many variations of passages of Lorem ipsum available, but the majority have suffered alteration in some form, by injected humour.&quot;
                            </p>
                            <div className="flex items-center justify-start">
                                <div className="w-10 h-10 bg-gray-200 rounded-full mr-3"></div>
                                <div className="text-left">
                                    <p className="font-semibold">Claro R. Almon</p>
                                    <p className="text-sm text-gray-500">Ullam Design</p>
                                </div>
                            </div>
                        </div>
                    </div>

                    {/* Indicateurs de pagination */}
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
            <section className="py-16 mb-20 bg-amber-50">
                <div className="container mx-auto px-4">
                    <div className="grid md:grid-cols-2 gap-8 items-center">
                        {/* Image div */}
                        <div className="ms-5">
                            <Image
                                src="/images/acceuil/about.svg"
                                alt="Education professionals"
                                width={384} // max-w-sm = 384px (tailwind par défaut)
                                height={300}

                                className="rounded-3xl w-full max-w-sm h-[300px] object-cover shadow-lg"
                            />
                        </div>


                        {/* Text content */}
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
                                    <p className="text-sm text-gray-600 mb-4 w-70">
                                        <strong className="block mb-2">OUR MISSION:</strong>
                                        Suspendisse ultrice gravida dictum fusce placerat ultricies integer quis auctor elit sed vulputate mi sit.
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
                                    <p className="text-sm text-gray-600 mb-4 w-70">
                                        <strong className="block mb-2">OUR VISSION:</strong>
                                        Suspendisse ultrice gravida dictum fusce placerat ultricies integer quis auctor elit sed vulputate mi sit.
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
                                Admission Open
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
            <footer className="bg-[#4D2C5E] text-white py-12">
                <div className="container mx-auto px-4">
                    <div className="text-center">
                        <p>© 2023 QualiTrack. All rights reserved.</p>
                    </div>
                </div>
            </footer>
        </main>
    )
}


