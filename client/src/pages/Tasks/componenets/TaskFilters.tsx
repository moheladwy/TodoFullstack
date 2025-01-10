import { Input } from "@/components/ui/input"
import { TaskPriority } from "@/lib/api/interfaces"
import {
  Select,
  SelectContent,
  SelectItem,
  SelectTrigger,
  SelectValue,
} from "@/components/ui/select"

export enum SortOption {
  NameAsc = "name_asc",
  NameDesc = "name_desc",
  PriorityLowToHigh = "priority_low_to_high",
  PriorityHighToLow = "priority_high_to_low",
}

interface TaskFiltersProps {
  onSearchChange: (search: string) => void
  onPriorityChange: (priority: string) => void
  onStatusChange: (status: string) => void
  onSortChange: (sort: SortOption) => void
}

export function TaskFilters({
  onSearchChange,
  onPriorityChange,
  onStatusChange,
  onSortChange,
}: TaskFiltersProps) {
  return (
    <div className="flex flex-wrap gap-2 mb-4">
      <Input
        placeholder="Search tasks..."
        className="flex-1 min-w-[200px]"
        onChange={(e) => onSearchChange(e.target.value)}
      />
      
      <Select defaultValue="all"
        onValueChange={(value) => onPriorityChange(value as string)}
      >
        <SelectTrigger className="w-[180px]">
          <SelectValue placeholder="Priority" />
        </SelectTrigger>
        <SelectContent>
          <SelectItem value="all">All Priorities</SelectItem>
          {Object.entries(TaskPriority)
            .filter(([key]) => isNaN(Number(key)))
            .map(([key, value]) => (
              <SelectItem key={value} value={value.toString()}>
                {key}
              </SelectItem>
            ))}
        </SelectContent>
      </Select>

      <Select defaultValue="all" 
        onValueChange={(value) => onStatusChange(value as string)}
      >
        <SelectTrigger className="w-[180px]">
          <SelectValue placeholder="Status" />
        </SelectTrigger>
        <SelectContent>
          <SelectItem value="all">All Status</SelectItem>
          <SelectItem value="completed">Completed</SelectItem>
          <SelectItem value="pending">Pending</SelectItem>
        </SelectContent>
      </Select>
      
      <Select 
        defaultValue={SortOption.NameAsc}
        onValueChange={(value) => onSortChange(value as SortOption)}
      >
        <SelectTrigger className="w-[180px]">
          <SelectValue placeholder="Sort by" />
        </SelectTrigger>
        <SelectContent>
          <SelectItem value={SortOption.NameAsc}>Name (A-Z)</SelectItem>
          <SelectItem value={SortOption.NameDesc}>Name (Z-A)</SelectItem>
          <SelectItem value={SortOption.PriorityLowToHigh}>
            Priority (Low to High)
          </SelectItem>
          <SelectItem value={SortOption.PriorityHighToLow}>
            Priority (High to Low)
          </SelectItem>
        </SelectContent>
      </Select>
    </div>
  )
}
