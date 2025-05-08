import { Bell, Mail, Search } from "lucide-react"
import { Avatar, AvatarFallback, AvatarImage } from "@/components/ui/avatar"
import { Button } from "@/components/ui/button"
import { Input } from "@/components/ui/input"

export function SearchHeader() {
    return (
        <div className="mb-8 bg-white rounded-3xl">
            <div className="flex items-center justify-between px-10 py-4">
                <h3 className="text-md font-semibold">Welcome to CPS</h3>
                <div className="flex items-center gap-4">
                    <div className="relative">
                        <Input placeholder="Search" className="w-74 rounded-full bg-gray-100 pl-4 pr-10 border-0" />
                        <Button size="icon" variant="ghost" className="absolute right-0 top-0 h-full rounded-full">
                            <Search className="h-4 w-4" />
                        </Button>
                    </div>
                    <Button size="icon" variant="outline" className="rounded-full">
                        <Mail className="h-4 w-4" />
                        <span className="absolute -right-1 -top-1 flex h-4 w-4 items-center justify-center rounded-full bg-red-500 text-[10px] text-white">
              2
            </span>
                    </Button>
                    <Button size="icon" variant="outline" className="rounded-full">
                        <Bell className="h-4 w-4" />
                        <span className="absolute -right-1 -top-1 flex h-4 w-4 items-center justify-center rounded-full bg-red-500 text-[10px] text-white">
              3
            </span>
                    </Button>
                    <Avatar>
                        <AvatarImage src="/images/profil.svg" />
                        <AvatarFallback>JD</AvatarFallback>
                    </Avatar>
                </div>
            </div>
        </div>
    )
}
