pipeline {
    agent {
        label "builder"
    }
    environment {
        DOCKER_REGISTRY = "10.227.222.197:30001"
        NAMESPACE = "ava-club"
        Version = "3.0.0"
    }
    parameters {
        booleanParam(name: 'BarghBeta', defaultValue: false, description: 'Should Deploy on Bargh Beta?')
    }
    stages {
	    stage("build"){
            steps {
                slackSend color: "good", message: "UserManagement build started - ${env.JOB_NAME} ${env.BUILD_NUMBER} (<${env.BUILD_URL}|Open>)"
                updateGitlabCommitStatus(name: "build", state: "running")
                sh "docker compose build UserManagement"
                sh "docker compose push UserManagement"
            }
            post {
                success {
                    updateGitlabCommitStatus(name: "build", state: "success")
		            slackSend color: "good", message: "UserManagement built - ${env.JOB_NAME} ${env.BUILD_NUMBER} (<${env.BUILD_URL}|Open>)"
                }
                unsuccessful {
                    updateGitlabCommitStatus(name: "build", state: "failed")
		            slackSend color: "danger", message: "UserManagement build failed! - ${env.JOB_NAME} ${env.BUILD_NUMBER} (<${env.BUILD_URL}|Open>)"
                }
                aborted {
                    updateGitlabCommitStatus(name: "build", state: "canceled")
   		            slackSend color: "warning", message: "UserManagement build aborted! - ${env.JOB_NAME} ${env.BUILD_NUMBER} (<${env.BUILD_URL}|Open>)"
                }
            }
        }
        stage("deploy") {
            agent {
                label "ava-club-runner"
            }
            steps {
                updateGitlabCommitStatus(name: "deploy", state: "running")
                sh "kubectl apply -f ./usermanagement.yml"
                sh "kubectl rollout restart -n ${env.NAMESPACE} deploy/usermanagement3-dep"
            }
            post {
                success {
                    updateGitlabCommitStatus(name: "deploy", state: "success")
		            slackSend color: "good", message: "UserManagement deployed - ${env.JOB_NAME} ${env.BUILD_NUMBER} (<${env.BUILD_URL}|Open>)"
                }
                unsuccessful {
                    updateGitlabCommitStatus(name: "deploy", state: "failed")
		            slackSend color: "danger", message: "UserManagement deploy failed - ${env.JOB_NAME} ${env.BUILD_NUMBER} (<${env.BUILD_URL}|Open>)"
                }
                aborted {
                    updateGitlabCommitStatus(name: "deploy", state: "canceled")
		            slackSend color: "warning", message: "UserManagement deploy aborted - ${env.JOB_NAME} ${env.BUILD_NUMBER} (<${env.BUILD_URL}|Open>)"
                }
            }
        }
        stage("deploy bargh beta") {
            when {
                expression {
                    return params.BarghBeta;
                }
            }
            agent {
                label "ava-club-runner"
            }
            steps {
                sh "kubectl apply -f ./usermanagement-conf.yml"
                sh "kubectl apply -f ./usermanagement.yml"
                sh "kubectl rollout restart -n bargh-club deploy/usermanagement3-dep"
            }
        }
    }
}

