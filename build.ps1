properties {
	$product		= 'FluentSecurity.Glimpse'
	$version		= '2.0.0'
	$label			= ''
	$configuration	= 'release'
	$useVerbose		= $false

	$artifactsName	= "$product-$version-$configuration" -replace "\.","_"

	$rootDir		= '.'
	$sourceDir		= "$rootDir\Source"
	$buildDir		= "$rootDir\Build"
	$artifactsDir	= "$buildDir\Artifacts\$artifactsName"
	$deploymentDir	= ''

	$buildNumber	= $null

	$setupMessage	= 'Executed Setup!'
	$cleanMessage	= 'Executed Clean!'
	$compileMessage	= 'Executed Compile!'
	$testMessage	= 'Executed Test!'
	$packMessage	= 'Executed Pack!'
	$deployMessage	= 'Executed Deploy!'
}

task default -depends Run

task Run {
	if ($branch -ne $null) {
		$branch = $branch -replace "refs/heads/",""
	} else {
		try { $branch = (git rev-parse --abbrev-ref HEAD) } catch { $branch = 'unknown' }
	}
	
	$script:buildId			= ([guid]::NewGuid().ToString().Substring(0,5))
	$script:buildVersion	= ?: {$buildNumber -ne $null} {"$version.$buildNumber"} {"$version"}
	$script:buildLabel		= ?: {$label -ne $null -and $label -ne ''} {"$version-$label"} {"$version"}
	
	if ($branch -ne 'master') {
		$script:buildLabel	= ?: {$buildNumber -ne $null} {"$buildLabel-build$buildNumber"} {"$buildLabel-buildx$buildId"}
	}
	
	# SemVer : 2.0.0-alpha.4+build.3 | 2.0.0-alpha.4
	# NuGet  : 2.0.0-alpha4-build3 | 2.0.0-alpha4
	
	# SemVer : 2.0.0+build.3 | 2.0.0
	# NuGet  : 2.0.0-build3 | 2.0.0

	Write-Host "Running build" -fore Yellow
	Write-Host "Product:        $product" -fore Yellow
	Write-Host "Version:        $version" -fore Yellow
	Write-Host "Label:          $label" -fore Yellow
	Write-Host "Build version:  $buildVersion" -fore Yellow
	Write-Host "Build label:    $buildLabel" -fore Yellow
	Write-Host "Branch:         $branch" -fore Yellow
	
	Invoke-Task Deploy
}

task Setup {
	nuget_exe install "$sourceDir\.nuget\packages.config" -outputdirectory "$sourceDir\packages"
	$setupMessage
}

task Clean {
	delete_directory $artifactsDir
	create_directory $artifactsDir
	$cleanMessage
}

task Compile -depends Setup, Clean {
	build_solution "$sourceDir\$product.sln"
	$compileMessage
}

task Test -depends Compile {
	$testMessage
}

task Pack -depends Test {
	pack_solution "$sourceDir\$product.sln" $artifactsDir $artifactsName
	$packMessage
}

task Deploy -depends Pack {
	$deployMessage
}

task ? -Description "Help" {
	Write-Documentation
}

taskSetup {
	$script:buildVersion = ?: {$buildNumber -ne $null} {"$version.$buildNumber"} {$version}
}
