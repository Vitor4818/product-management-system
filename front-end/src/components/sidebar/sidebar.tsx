"use client";
import Image from "next/image";
import Icon from "@/assets/icon.png";



export function SideBar() {
 
  return (
    <div className="bg-gray-400 w-96 h-screen p-6 bg-white">
    
    
    <div className="flex flex-row gap-2.5 items-center">
    <Image src={Icon} alt="" width={50} height={50} className="rounded-2xl"/>
    <p className="text-indigo-800 text-3xl font-medium">Shop<span className="text-black">sense</span></p>
    </div>

    <div>

        


    </div>







    </div>
  
  )
}