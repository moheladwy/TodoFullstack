pipeline {
    agent any

    environment {
        // Replace with your Docker Hub credentials stored in Jenkins
        DOCKER_CREDENTIALS_ID = 'DOCKER_HUB_ID'                       // Jenkins credentials ID for Docker Hub.
        DOCKER_IMAGE = 'only1adwy/todo-api'                           // Your Docker Hub repo.
        BRANCH = 'main'                                               // Branch to build.
        REPO_URL = 'https://github.com/moheladwy/TodoFullstack.git'   // GitHub repo URL.
        BUILD_TAG = "V1.8.${env.BUILD_NUMBER}"                        // Build tag.
    }

    stages {
        stage('Checkout') {
            steps {
                // Checkout the source code from GitHub
                git branch: "${BRANCH}", url: "${REPO_URL}"
            }
        }
        
        stage('Build Docker Image') {
            steps {
                script {
                    // Build Docker image using the Dockerfile in the repo
                    sh 'docker build -t ${DOCKER_IMAGE}:${BUILD_TAG} .'
                }
            }
        }

        stage('Tag Docker Image as latest') {
            steps {
                script {
                    // Tag the built image as latest
                    sh 'docker tag ${DOCKER_IMAGE}:${BUILD_TAG} ${DOCKER_IMAGE}:latest'
                }
            }
        }
        
        stage('Login to Docker Hub') {
            steps {
                script {
                    // Login to Docker Hub using Jenkins credentials
                    withCredentials([usernamePassword(credentialsId: DOCKER_CREDENTIALS_ID, passwordVariable: 'DOCKER_PASSWORD', usernameVariable: 'DOCKER_USERNAME')]) {
                        sh 'echo "$DOCKER_PASSWORD" | docker login -u "$DOCKER_USERNAME" --password-stdin'
                    }
                }
            }
        }
        
        stage('Push Docker Image to Docker Hub') {
            steps {
                script {
                    // Push the built image to Docker Hub
                    sh 'docker push ${DOCKER_IMAGE}:${BUILD_TAG}'
                    sh 'docker push ${DOCKER_IMAGE}:latest'
                }
            }
        }
    }

    post {
        success {
            echo 'Build and Docker Push completed successfully!'
        }
        failure {
            echo 'Build or Docker Push failed.'
        }
    }
}
