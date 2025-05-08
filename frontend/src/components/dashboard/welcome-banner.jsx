import { Bell, Mail, Search } from "lucide-react"
import Image from "next/image"
import { Button } from "@/components/ui/button"
import { Input } from "@/components/ui/input"
import { Avatar, AvatarFallback, AvatarImage } from "@/components/ui/avatar"

export function WelcomeBanner({fullName}) {
    return (
        <div className="mb-8">
            {/*<div className="flex items-center justify-between">*/}
            {/*    <h1 className="text-xl font-semibold">Welcome to CPS</h1>*/}
            {/*    <div className="flex items-center gap-4">*/}
            {/*        <div className="relative">*/}
            {/*            <Input placeholder="Search" className="w-64 rounded-full bg-gray-100 pl-4 pr-10" />*/}
            {/*            <Button size="icon" variant="ghost" className="absolute right-0 top-0 h-full rounded-full">*/}
            {/*                <Search className="h-4 w-4" />*/}
            {/*            </Button>*/}
            {/*        </div>*/}
            {/*        <Button size="icon" variant="outline" className="rounded-full">*/}
            {/*            <Mail className="h-4 w-4" />*/}
            {/*        </Button>*/}
            {/*        <Button size="icon" variant="outline" className="rounded-full">*/}
            {/*            <Bell className="h-4 w-4" />*/}
            {/*        </Button>*/}
            {/*        <Avatar>*/}
            {/*            <AvatarImage src="/placeholder-user.jpg" />*/}
            {/*            <AvatarFallback>JD</AvatarFallback>*/}
            {/*        </Avatar>*/}
            {/*    </div>*/}
            {/*</div>*/}

            <div className="mt-6 overflow-hidden rounded-xl bg-gradient-to-r from-[#866BA2] to-[#4D2C5E] text-white">
                <div className="flex items-center justify-between ">
                    <div className="space-y-2 p-10 ms-3">
                        <p className="text-sm text-white/80">September 4, 2023</p>
                        <h2 className="text-2xl font-bold">Welcome back, {fullName}!</h2>
                        <p className="text-sm">Always stay updated in your student portal</p>
                    </div>
                    {/*<div className="relative h-40 w-64">*/}
                    {/*</div>*/}
                    <div className="flex pt-10 pe-6 me-40 ">
                        <Image
                            src="/images/dashboard/schoolcap.svg"
                            alt="Student illustration"
                            width={170}
                            height={100}
                            className="object-contain "
                        />
                        <Image
                            src="/images/dashboard/student.svg"
                            alt="Student illustration"
                            width={170}
                            height={130}
                            className="object-contain "
                        />
                    </div>


                </div>
            </div>
        </div>
    )
}
