import { cn } from "@/lib/utils"

interface LoadingProps extends React.HTMLAttributes<HTMLDivElement> {
  size?: 'sm' | 'md' | 'lg'
  text?: string
  fullScreen?: boolean
}

export function Loading({ 
  size = 'md', 
  text = 'Loading...', 
  fullScreen = false,
  className,
  ...props 
}: LoadingProps) {
  const sizeClasses = {
    sm: 'w-5 h-5 border-2',
    md: 'w-8 h-8 border-3',
    lg: 'w-12 h-12 border-4'
  }

  return (
    <div 
      className={cn(
        "flex flex-col items-center justify-center gap-3",
        fullScreen && "fixed inset-0 bg-background/80 backdrop-blur-sm",
        className
      )}
      {...props}
    >
      <div 
        className={cn(
          "animate-spin rounded-full border-t-primary",
          "border-l-transparent border-r-transparent border-b-transparent",
          sizeClasses[size]
        )} 
      />
      {text && (
        <p className={cn(
          "text-muted-foreground",
          size === 'sm' && "text-sm",
          size === 'lg' && "text-lg"
        )}>
          {text}
        </p>
      )}
    </div>
  )
}

// Optional: Export variations for common use cases
export function LoadingFullScreen() {
  return <Loading fullScreen size="lg" />
}

export function LoadingSpinner() {
  return <Loading text={undefined} />
}

export function LoadingButton() {
  return <Loading size="sm" text="Loading..." className="flex-row gap-2" />
}