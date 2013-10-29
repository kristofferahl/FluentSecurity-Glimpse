properties {
	$product		= 'FluentSecurity.Glimpse'
	$version		= '2.0.0'
	$configuration	= 'release'
	$useVerbose		= $false

	$rootDir		= '.'
	$sourceDir		= "$rootDir\Source"
	$buildDir		= "$rootDir\Build"
	$artifactsDir	= "$buildDir\Artifacts"
	$artifactsName	= "$product-$version-$configuration" -replace "\.","_"
	$deploymentDir	= ''
	
	$buildNumber	= $null
	
	$setupMessage	= 'Executed Setup!'
	$cleanMessage	= 'Executed Clean!'
	$compileMessage	= 'Executed Compile!'
	$testMessage	= 'Executed Test!'
	$packMessage	= 'Executed Pack!'
	$deployMessage	= 'Executed Deploy!'
}

task default -depends Local

task Local {
	Write-Host "Running local build" -fore Yellow
	Write-Host "Product:        $product" -fore Yellow
	Write-Host "Version:        $version" -fore Yellow
	Write-Host "Build version:  $buildVersion" -fore Yellow
	Invoke-Task Deploy
}
task Release {
	Write-Host "Running release build" -fore Yellow
	Write-Host "Product:        $product" -fore Yellow
	Write-Host "Version:        $version" -fore Yellow
	Write-Host "Build version:  $buildVersion" -fore Yellow
	Invoke-Task Deploy
}

task Setup {
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
	if ($deploymentDir -ne $null -and $deploymentDir -ne "") {
		Write-Host "Deploying to: $deploymentDir."
	} else {
		Write-Host "No deployment directory set!"
	}
	$deployMessage
}

task ? -Description "Help" {
	Write-Documentation
}

taskSetup {
	$script:buildVersion = ?: {$buildNumber -ne $null} {"$version.$buildNumber"} {$version}
}
