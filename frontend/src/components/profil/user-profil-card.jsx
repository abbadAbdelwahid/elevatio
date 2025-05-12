import { Card, CardContent } from "@/components/ui/card"
import Image from "next/image"

export function UserProfileCard({user}) {
    return (
        <Card className="overflow-hidden border-0 shadow-lg bg-white ">
            <CardContent className="flex flex-col items-center p-8">
                <div className="relative mb-6 h-40 w-40 overflow-hidden rounded-full border-4 border-purple-100">
                    <Image
                        src={user.image}
                        alt="User profile"
                        width={160}
                        height={160}
                        className="object-cover"
                    />
                </div>
                <h2 className="mb-2 text-xl font-semibold text-[#4a2a5a]">{user.firstName}</h2>
                <p className="text-md text-gray-500">{user.email}</p>
            </CardContent>
        </Card>
    )
}