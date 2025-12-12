import { Home, Box, Grid2X2 } from "lucide-react";
import Image from "next/image";
import Icon from "@/assets/icon.png";

import {
  Sidebar,
  SidebarContent,
  SidebarFooter,
  SidebarGroup,
  SidebarGroupContent,
  SidebarGroupLabel,
  SidebarHeader,
  SidebarMenu,
  SidebarMenuButton,
  SidebarMenuItem,
} from "@/components/ui/sidebar";
import {
  Card,
  CardAction,
  CardContent,
  CardDescription,
  CardFooter,
  CardHeader,
  CardTitle,
} from "../ui/card";
import { Button } from "../ui/button";

// Menu items.
const items = [
  {
    title: "Produtos",
    url: "/products",
    icon: Box,
  },
  {
    title: "Categorias",
    url: "/categories",
    icon: Grid2X2,
  },
];

const generalItems = [
  {
    title: "Dashboard",
    url: "/dashboard",
    icon: Home,
  },
];
export function AppSidebar() {
  return (
    <Sidebar className="p-2 bg-gray-100 [&>*]:bg-gray-100">
      <SidebarHeader className="flex flex-row items-center">
        <Image
          src={Icon}
          alt={"Logo aplicação"}
          width={50}
          height={50}
          className="rounded-2xl"
        />
        <h1 className="flex flex-row text-violet-600 font-bold text-2xl">
          Shop<p className="text-black">Sense</p>
        </h1>
      </SidebarHeader>
      <SidebarContent className="mt-4">
        <SidebarGroup>
          <SidebarGroupLabel className="text-lg">General</SidebarGroupLabel>
          <SidebarGroupContent>
            <SidebarMenu>
              <div className="ml-3 p-1">
                {generalItems.map((item) => (
                  <SidebarMenuItem key={item.title}>
                    <SidebarMenuButton asChild>
                      <a href={item.url}>
                        <item.icon className="!w-5 !h-5 text-indigo-500" />
                        <span>{item.title}</span>
                      </a>
                    </SidebarMenuButton>
                  </SidebarMenuItem>
                ))}
              </div>
              <SidebarGroupLabel className="text-lg">Shop</SidebarGroupLabel>
              <div className=" flex flex-col gap-3 ml-3 p-1">
                {items.map((item) => (
                  <SidebarMenuItem key={item.title}>
                    <SidebarMenuButton asChild>
                      <a href={item.url}>
                        <item.icon className="!w-5 !h-5 text-indigo-500" />
                        <span>{item.title}</span>
                      </a>
                    </SidebarMenuButton>
                  </SidebarMenuItem>
                ))}
              </div>
            </SidebarMenu>
          </SidebarGroupContent>
        </SidebarGroup>
      </SidebarContent>

      <SidebarFooter>
        <SidebarMenu>




          <Card className="w-full">
            <CardHeader>
              <CardTitle>Experimente ShopSense Pro</CardTitle>
              <CardDescription>Obtenha o Pro e aproveite mais de 20 recursos para aumentar suas vendas. Teste grátis por 30 dias!</CardDescription>
            </CardHeader>

            <CardContent>
              <Button className="w-full rounded-4xl cursor-pointer h-12">
                Planos de atualização
              </Button>
            </CardContent>


          </Card>





        </SidebarMenu>
      </SidebarFooter>
    </Sidebar>
  );
}
