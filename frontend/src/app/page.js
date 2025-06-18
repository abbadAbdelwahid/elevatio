import Image from "next/image";
import Link from "next/link"
import ImageCarousel from "@/components/layout/carouselComponent";
import NavComponent from "@/components/layout/navComponent";


export default function Home() {
    return (
        <main className="min-h-screen">
            <NavComponent />

            {/* Hero Section */}
            <section id="home" className="bg-amber-50 py-16 relative overflow-hidden">
                <div className="container mx-auto px-4 grid md:grid-cols-2 gap-8 items-center">
                    <div className="space-y-6 z-10">
                        <div className="space-y-3">
                            <div className="space-y-3 mt-8"> {/* Adding top margin */}
                            <h2 className="text-4xl md:text-5xl font-bold text-gray-900">
                                Elevatio is here to elevate the quality of training through seamless evaluations <br />
                                in Higher Education
                            </h2>
                            </div>
                            <h2 className="text-4xl md:text-5xl font-bold text-orange-500">Elevatio</h2>
                        </div>
                        <p className="text-gray-700 max-w-md">
                            A smart platform to automate, collect, and analyze evaluations for training programs in universities and engineering schools.
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
                                        Students and faculty can share feedback with just a few clicks, helping institutions continuously improve the quality of education and training.
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
                    <h2 className="text-3xl font-bold mb-2">Plateform Preview</h2>
                    {/*<p className="text-gray-500 mb-8">Lorem ipsum is simply dummy text of the printing.</p>*/}

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
            <section id="testimonials" className="py-20 bg-white">
                <div className="container px-4 mx-auto text-center">
                    <h2 className="text-3xl font-bold mb-2">What Our Students Say</h2>
                    <p className="text-gray-500 mb-12">See how Elevatio is helping to improve training evaluations and enhance educational experiences at ENSAT.</p>

                    <div className="grid md:grid-cols-3 gap-4">
                        {/* First Testimonial */}
                        <div className="bg-white p-6 rounded-lg shadow-md">
                            <p className="text-gray-600 mb-4 italic">
                                &quot;Elevatio has streamlined the evaluation process and made it easier for us to provide feedback. It&#39;s a powerful tool for improving the quality of our courses.&quot;
                            </p>
                            <div className="flex items-center justify-start">
                                <div className="w-10 h-10 bg-gray-200 rounded-full mr-3"></div>
                                <div className="text-left">
                                    <p className="font-semibold">Anass El Ouahabi</p>
                                    <p className="text-sm text-gray-500">Student, ENSAT</p>
                                </div>
                            </div>
                        </div>

                        {/* Second Testimonial */}
                        <div className="bg-white p-6 rounded-lg shadow-md">
                            <p className="text-gray-600 mb-4 italic">
                                &quot;Thanks to Elevatio, submitting my course feedback has never been easier. The platform is intuitive and ensures my voice is heard.&quot;
                            </p>
                            <div className="flex items-center justify-start">
                                <div className="w-10 h-10 bg-gray-200 rounded-full mr-3"></div>
                                <div className="text-left">
                                    <p className="font-semibold">Abdelouahed Abbad</p>
                                    <p className="text-sm text-gray-500">Student, ENSAT</p>
                                </div>
                            </div>
                        </div>

                        {/* Third Testimonial */}
                        <div className="bg-white p-6 rounded-lg shadow-md">
                            <p className="text-gray-600 mb-4 italic">
                                &quot;Elevatio has made evaluating our courses quick and effective. The feedback is detailed, and it really helps us as students influence course quality.&quot;
                            </p>
                            <div className="flex items-center justify-start">
                                <div className="w-10 h-10 bg-gray-200 rounded-full mr-3"></div>
                                <div className="text-left">
                                    <p className="font-semibold">Yasser Boulerbah</p>
                                    <p className="text-sm text-gray-500">Student, ENSAT</p>
                                </div>
                            </div>
                        </div>

                        {/* Fourth Testimonial */}
                        <div className="bg-white p-6 rounded-lg shadow-md">
                            <p className="text-gray-600 mb-4 italic">
                                &quot;With Elevatio, I can easily give feedback after every course. It&#39;s great to see the improvements based on the evaluations we provide.&quot;
                            </p>
                            <div className="flex items-center justify-start">
                                <div className="w-10 h-10 bg-gray-200 rounded-full mr-3"></div>
                                <div className="text-left">
                                    <p className="font-semibold">Wail Hadad</p>
                                    <p className="text-sm text-gray-500">Student, ENSAT</p>
                                </div>
                            </div>
                        </div>

                        {/* Fifth Testimonial */}
                        <div className="bg-white p-6 rounded-lg shadow-md">
                            <p className="text-gray-600 mb-4 italic">
                                &quot;I love how easy Elevatio makes it to track and submit feedback. It&#39;s efficient, and I can see how it helps in enhancing course content.&quot;
                            </p>
                            <div className="flex items-center justify-start">
                                <div className="w-10 h-10 bg-gray-200 rounded-full mr-3"></div>
                                <div className="text-left">
                                    <p className="font-semibold">Oussama El Batteoui</p>
                                    <p className="text-sm text-gray-500">Student, ENSAT</p>
                                </div>
                            </div>
                        </div>

                        {/* Sixth Testimonial */}
                        <div className="bg-white p-6 rounded-lg shadow-md">
                            <p className="text-gray-600 mb-4 italic">
                                &quot;Elevatio has really made it easier to evaluate courses and see tangible improvements. It&#39;s a great tool for students and educators alike.&quot;
                            </p>
                            <div className="flex items-center justify-start">
                                <div className="w-10 h-10 bg-gray-200 rounded-full mr-3"></div>
                                <div className="text-left">
                                    <p className="font-semibold">Mohamed Bounouar</p>
                                    <p className="text-sm text-gray-500">Student, ENSAT</p>
                                </div>
                            </div>
                        </div>
                    </div>

                    {/* Pagination Indicators */}
                    <div className="flex justify-center mt-8 gap-1">
                        <div className="w-8 h-2 bg-purple-800 rounded-full"></div>
                        <div className="w-2 h-2 bg-gray-300 rounded-full"></div>
                        <div className="w-2 h-2 bg-gray-300 rounded-full"></div>
                    </div>
                </div>
            </section>



            {/* About Section */}
            <section id="about" className="py-16 bg-white">
                <div className="container mx-auto px-4 text-center">
                    <h2 className="text-3xl font-bold mb-2">About Elevatio</h2>
                    <p className="text-gray-500 mb-8">
                        Elevatio is a cutting-edge platform designed to revolutionize the way educational institutions evaluate and improve their training programs. In today’s fast-paced academic world, managing feedback and assessments manually can be time-consuming and error-prone. Elevatio automates the entire process, allowing universities and engineering schools to easily collect, analyze, and act on feedback from students, professors, and administrators.
                    </p>
                    <p className="text-gray-500 mb-12">
                        Our platform offers intuitive tools for students to provide detailed evaluations, while enabling instructors to receive actionable insights. Administrators gain access to comprehensive reports and analytics that help in making data-driven decisions to improve training quality and student satisfaction. With Elevatio, educational institutions can ensure a continuous cycle of improvement, enhancing the overall learning experience for everyone involved.
                    </p>
                </div>
            </section>


            {/*/!* Benefits Section *!/*/}
            {/*<section className="py-16 mb-20 bg-amber-50">*/}
            {/*    <div className="container mx-auto px-4">*/}
            {/*        <div className="grid md:grid-cols-2 gap-8 items-center">*/}
            {/*            /!* Image div *!/*/}
            {/*            <div className="ms-5">*/}
            {/*                <Image*/}
            {/*                    src="/images/acceuil/about.svg"*/}
            {/*                    alt="Education professionals"*/}
            {/*                    width={384} // max-w-sm = 384px (tailwind par défaut)*/}
            {/*                    height={300}*/}

            {/*                    className="rounded-3xl w-full max-w-sm h-[300px] object-cover shadow-lg"*/}
            {/*                />*/}
            {/*            </div>*/}


            {/*            /!* Text content *!/*/}
            {/*            <div>*/}
            {/*                <h2 className="text-3xl font-bold mb-6">*/}
            {/*                    <span>Benefit From Our Online</span>*/}
            {/*                    <br />*/}
            {/*                    <span>Learning Expertise Earn</span>*/}
            {/*                    <br />*/}
            {/*                    <span className="text-pink-500">Professional</span>*/}
            {/*                </h2>*/}

            {/*                <div className="grid grid-cols-2 gap-6 mb-8">*/}
            {/*                    <div>*/}
            {/*                        <p className="text-sm text-gray-600 mb-4 w-70">*/}
            {/*                            <strong className="block mb-2">OUR MISSION:</strong>*/}
            {/*                            Suspendisse ultrice gravida dictum fusce placerat ultricies integer quis auctor elit sed vulputate mi sit.*/}
            {/*                        </p>*/}
            {/*                        <div className="flex items-center gap-2">*/}
            {/*                            <svg*/}
            {/*                                xmlns="http://www.w3.org/2000/svg"*/}
            {/*                                width="20"*/}
            {/*                                height="20"*/}
            {/*                                viewBox="0 0 24 24"*/}
            {/*                                fill="none"*/}
            {/*                                stroke="currentColor"*/}
            {/*                                strokeWidth="2"*/}
            {/*                                strokeLinecap="round"*/}
            {/*                                strokeLinejoin="round"*/}
            {/*                                className="text-green-500"*/}
            {/*                            >*/}
            {/*                                <polyline points="20 6 9 17 4 12" />*/}
            {/*                            </svg>*/}
            {/*                            <p className="text-sm">Professional certification</p>*/}
            {/*                        </div>*/}
            {/*                    </div>*/}

            {/*                    <div>*/}
            {/*                        <p className="text-sm text-gray-600 mb-4 w-70">*/}
            {/*                            <strong className="block mb-2">OUR VISSION:</strong>*/}
            {/*                            Suspendisse ultrice gravida dictum fusce placerat ultricies integer quis auctor elit sed vulputate mi sit.*/}
            {/*                        </p>*/}
            {/*                        <div className="flex items-center gap-2">*/}
            {/*                            <svg*/}
            {/*                                xmlns="http://www.w3.org/2000/svg"*/}
            {/*                                width="20"*/}
            {/*                                height="20"*/}
            {/*                                viewBox="0 0 24 24"*/}
            {/*                                fill="none"*/}
            {/*                                stroke="currentColor"*/}
            {/*                                strokeWidth="2"*/}
            {/*                                strokeLinecap="round"*/}
            {/*                                strokeLinejoin="round"*/}
            {/*                                className="text-green-500"*/}
            {/*                            >*/}
            {/*                                <polyline points="20 6 9 17 4 12" />*/}
            {/*                            </svg>*/}
            {/*                            <p className="text-sm">Flexible learning schedule</p>*/}
            {/*                        </div>*/}
            {/*                    </div>*/}
            {/*                </div>*/}

            {/*                <button className="bg-teal-500 hover:bg-teal-600 text-white rounded-full px-6 py-2 flex items-center">*/}
            {/*                    Admission Open*/}
            {/*                    <svg*/}
            {/*                        xmlns="http://www.w3.org/2000/svg"*/}
            {/*                        width="16"*/}
            {/*                        height="16"*/}
            {/*                        viewBox="0 0 24 24"*/}
            {/*                        fill="none"*/}
            {/*                        stroke="currentColor"*/}
            {/*                        strokeWidth="2"*/}
            {/*                        strokeLinecap="round"*/}
            {/*                        strokeLinejoin="round"*/}
            {/*                        className="ml-2"*/}
            {/*                    >*/}
            {/*                        <polyline points="9 18 15 12 9 6" />*/}
            {/*                    </svg>*/}
            {/*                </button>*/}
            {/*            </div>*/}

            {/*        </div>*/}
            {/*    </div>*/}
            {/*</section>*/}
            {/* Contact Section */}
            <section id="contact" className="bg-purple-50 py-16">
                <div className="container mx-auto px-4 text-center">
                    <h2 className="text-3xl font-bold mb-6">Get in Touch with Us</h2>
                    <p className="text-gray-600 mb-8">
                        We are here to answer any questions you may have. Whether you&#39;re looking for more information on our platform or need support, feel free to reach out.
                    </p>

                    <div className="bg-white p-6 rounded-lg shadow-lg max-w-lg mx-auto">
                        <h3 className="text-xl font-semibold text-gray-800 mb-4">Contact us at</h3>
                        <p className="text-gray-700 mb-6">
                            For inquiries, support, or feedback, you can reach us directly via email at
                            <span className="mr-2"></span> {/* Adding space between "at" and email */}
                            <a href="mailto:contact@elevatio.me" className="text-purple-600 hover:underline">
                                contact@elevatio.me
                            </a>
                        </p>
                        <p className="text-gray-600">
                            Our team is always ready to assist you and provide the best possible solutions for your needs.
                        </p>
                    </div>
                </div>
            </section>


            {/* Footer */}
            <footer className="bg-[#4D2C5E] text-white py-12">
                <div className="container mx-auto px-4">
                    <div className="text-center">
                        <p>© 2025 Elevatio. All rights reserved.</p>
                    </div>
                </div>
            </footer>
        </main>
    )
}


